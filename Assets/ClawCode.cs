using UnityEngine;
using UnityEngine.Playables;
using VHS; // Necesario para PlayableDirector

public class ClawCode : MonoBehaviour
{
    [SerializeField] public bool activado;
    [SerializeField] private GameObject luces;
    [SerializeField] private GameObject gbcam;
    [SerializeField] private MoveObject moveObject;
    [SerializeField] private Transform destino;
    [SerializeField] private Transform camPlayer;
    [SerializeField] private GameObject PLAYER;
    [SerializeField] private GameObject guide;
    [SerializeField] private GameObject guidePoint;
    [SerializeField] private ClawController clawController;

    [SerializeField] private GameObject timeLineCinematica;
    [SerializeField] private GameObject gancho;

    [SerializeField] private PlayableDirector director;

    [SerializeField] private InteractableBase interactable;
    [SerializeField] private InteractableBase interactablePanel;
    [SerializeField] private InteractableBase interactableTrap;

    [SerializeField] private InteractionController interactionController;

    public InteractionInputData interactionInputData;

    public AudioSource asrc_claw;

    private void Awake()
    {
        // Buscar el PlayableDirector en el objeto de la timeline
        if (timeLineCinematica != null)
        {
            director = timeLineCinematica.GetComponent<PlayableDirector>();

            if (director != null)
                director.stopped += OnTimelineStopped; // Suscribir al evento
        }
    }

    private void Update()
    {
        luces.SetActive(activado);

        if (activado && !asrc_claw.isPlaying)
        {
            asrc_claw.Play();
        }
        else if (!activado && asrc_claw.isPlaying)
        {
            asrc_claw.Stop();
        }


        if (activado)
        {
            interactable.tooltipMessage = "'E' para usar.";
        }
        else
        {
            interactable.tooltipMessage = "";
        }

    }

    public void SetClawCam()
    {
        if (activado)
        {
            setclawCamTrue();
        }
    }

    private void setclawCamTrue()
    {
        timeLineCinematica.SetActive(true);

        gancho.SetActive(false);

        PLAYER.SetActive(false);
        guide.SetActive(false);
        guidePoint.SetActive(false);
    }

    public void DesactivarClaw()
    {
        activado = false;
    }

    private void OnTimelineStopped(PlayableDirector obj)
    {
        interactionController.PlayAnimDestornillador();
        gancho.SetActive(false);
        activado = false;
        PLAYER.SetActive(true);
        guide.SetActive(true);
        guidePoint.SetActive(true);
        interactable.isInteractable = false;
        interactable.tooltipMessage = "";

        interactablePanel.isInteractable = false;
        interactablePanel.tooltipMessage = "";

        interactableTrap.isInteractable = true;
        interactableTrap.tooltipMessage = "''E'' para desatornillar";

        interactionInputData.InteractedReleased = false;
        interactionInputData.InteractedReleased = true;

        interactionInputData.InteractedClicked = false;
        interactionInputData.InteractedClicked = true;


        Destroy(timeLineCinematica);
    }

    public void GetBack()
    {
        // lógica para volver al estado normal
    }
}
