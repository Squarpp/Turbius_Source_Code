using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform door;
    public Quaternion quat;
    public GameObject[] Lights;
    [SerializeField] private AudioSource lev;
    [SerializeField] private AudioSource fps;
    [SerializeField] private AudioSource audioNacho;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip clipturnoff;
    [SerializeField] private AudioClip cliphalflifenull;
    [SerializeField] private AudioClip ayudaNacho;
    [SerializeField] private VHS.InteractableBase inter;
    [SerializeField] private BoxCollider chapa;

    [SerializeField] private SMS sms;


    public bool funcionActivate;
    public Animator anim;

    public void Soundd()
    {
        lev.PlayOneShot(clip);
    }

    public void Activate()
    {
        if (funcionActivate)
        {
            ActivateON();
        }
        else
        {
           ActivateOFF();
        }
    }

    public void ActivateON()
    {
        anim.SetBool("on", true);
    }

    private void Push()
    {
        inter.tooltipMessage = "";
        chapa.enabled = true;

        sms.JejejeFunc();

        fps.PlayOneShot(clipturnoff);

        audioNacho.PlayOneShot(ayudaNacho);

        door.rotation = quat;

        PlayerPrefs.SetInt("level", 2);

        for (int i = 0; i < Lights.Length; i++)
        {
            Destroy(Lights[i]);
        }
    }
    public void ActivateOFF()
    {
        lev.PlayOneShot(cliphalflifenull);
    }
}
