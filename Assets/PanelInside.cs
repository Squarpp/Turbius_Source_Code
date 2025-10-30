using System.Collections;
using TMPro;
using UnityEngine;

public class PanelInside : MonoBehaviour
{
    [SerializeField] string pin;
    [SerializeField] string pineff;
    [SerializeField] TextMeshPro textMeshPro;
    [SerializeField] PaperCode pCode;

    [SerializeField] GameObject gTextNo;
    [SerializeField] GameObject gTextSi;
    [SerializeField] public bool canType;

    public GameObject guide;
    public GameObject guidePoint;
    public MoveObject moveObject;

    [SerializeField] private Transform destino;
    [SerializeField] private Transform camPlayer;

    private bool correct = false;

    [SerializeField] GameObject Panel;
    [SerializeField] GameObject PanelPin;

    [SerializeField] private PauseManager pauseManager;
    [SerializeField] public bool fixBug;

    [SerializeField] public AudioSource asrc;
    [SerializeField] public AudioClip[] clips;

    private void Update()
    {
        textMeshPro.text = pin + pineff;

        if (canType)
        {
            pineff = "_";
            if (pin.Length < 5)
            {
                for (int i = 0; i <= 9; i++)
                {
                    KeyCode alphaKey = KeyCode.Alpha0 + i;
                    KeyCode keypadKey = KeyCode.Keypad0 + i;

                    if (Input.GetKeyDown(alphaKey) || Input.GetKeyDown(keypadKey))
                    {
                        pin += i.ToString();
                        asrc.PlayOneShot(clips[0]);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && pin.Length > 0)
            {
                pin = pin.Substring(0, pin.Length - 1);
                asrc.PlayOneShot(clips[1]);
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !fixBug)
            {
                ReturnToFps();
            }

            if (Input.GetKeyDown(KeyCode.Return) && pin.Length > 0)
            {
                if (pin == pCode.pin)
                {
                    asrc.PlayOneShot(clips[3]);
                    gTextSi.SetActive(true);
                    correct = true;
                    StartCoroutine(returningFps());
                }
                else
                {
                    asrc.PlayOneShot(clips[4]);
                    gTextNo.SetActive(true);
                    correct = false;
                    StartCoroutine(returningFps());
                }
            }
        }
    }

    IEnumerator returningFps()
    {
        pineff = "";
        canType = false;
        yield return new WaitForSeconds(2.5f);

        if (correct)
        {
            asrc.PlayOneShot(clips[2]);
            Panel.SetActive(true);
            PanelPin.SetActive(false);
        }
        else
        {
             ReturnToFps();
        }
    }

    public void ReturnToFps()
    {
        pin = "";

        gTextNo.SetActive(false);
        gTextSi.SetActive(false);

        pauseManager.canPause = true;
        pauseManager.fixPause = 1;

        moveObject.SnapToTarget(destino, camPlayer, true);
        guide.SetActive(true);
        guidePoint.SetActive(true);

        fixBug = true;
    }
}
