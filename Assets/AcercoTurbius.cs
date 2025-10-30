using UnityEngine;

public class AcercoTurbius : MonoBehaviour
{
    public EnemyAI ai;
    public AIFreddy aiFreddy;

    public bool activeTurbius;
    public GameObject turbiusObj;

    public Transform sitio;
    public Transform teleporter;
    [SerializeField] private bool activ;
    public GameObject aiPlayer;
    public FieldOfView fieldOfView;
    public bool isFreddy;

    [Header("Audio")]
    public AudioClip triggerClip;  // clip asignado en el inspector
    [SerializeField] private AudioSource teleporterAudio;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !activ)
        {
            // reproducir el sonido desde el teleporter
            if (triggerClip != null && teleporterAudio != null)
                teleporterAudio.PlayOneShot(triggerClip);

            if (!isFreddy)
            {
                aiPlayer.SetActive(false);
                aiPlayer.transform.position = teleporter.position;
                aiPlayer.SetActive(true);
                fieldOfView.OnActive();
                ai.FollowSite(sitio);
                if (activeTurbius)
                {
                    turbiusObj.SetActive(true);
                }

                activ = true;
            }
            else
            {
                aiPlayer.SetActive(false);
                aiPlayer.transform.position = teleporter.position;
                aiPlayer.SetActive(true);
                fieldOfView.OnActive();
                aiFreddy.FollowSite(sitio);
                activ = true;
            }
        }
    }

    /* private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && activ)
        {
            activ = false;
        }
    }*/
}
