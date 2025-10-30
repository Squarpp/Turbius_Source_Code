using UnityEngine;

public class GameBoy : MonoBehaviour
{
    public GameObject PLAYER;
    public GameObject gbcam;
    public GameObject guide;
    public GameObject guidePoint;
    public GameObject pokemon;
    public GameObject screen;

    public MoveObject moveObject;
    public Transform destino;
    public Transform camPlayer;
    private BoxCollider2D box;
    public Animator lightanim;

    [SerializeField] private AudioSource asrc;
    [SerializeField] private AudioClip clipOn;

    [SerializeField] private Lever lever;
    [SerializeField] private VHS.InteractableBase inter;
    [SerializeField] private VHS.InteractableBase panel;

    public void SetScreenGB()
    {
        gbcam.SetActive(true);
        moveObject.SnapToTarget(camPlayer, destino, false);
        PLAYER.SetActive(false);
        guide.SetActive(false);
        guidePoint.SetActive(false);
        pokemon.SetActive(true);
        screen.SetActive(true);
    }
    public void GetBack()
    {
        panel.isInteractable = true;
        inter.tooltipMessage = "";
        inter.isInteractable = false;
        asrc.PlayOneShot(clipOn);
        box = GetComponent<BoxCollider2D>();
        if(box != null)
        {
            box.enabled = false;
        }
        lever.funcionActivate = true;
        lightanim.SetBool("on", true);
        moveObject.SnapToTarget(destino, camPlayer, true);
        guide.SetActive(true);
        guidePoint.SetActive(true);
        Destroy(pokemon);
        Destroy(screen);
    }
}
