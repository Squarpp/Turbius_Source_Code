using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CinematicaFinalSetup : MonoBehaviour
{
    public GameObject acto_1;
    public GameObject acto_2;

    public PlayableDirector acto_1_playable;
    public PlayableDirector acto_2_playable;

    public GameObject playerr;
    public GameObject brazoPlayer;
    public GameObject[] UIvisuals;

    public void StartCinematica()
    {
        // Vincular evento al terminar acto 1
        acto_1_playable.stopped += OnActo1Terminado;

        // Activar acto 1 y reproducir su timeline
        acto_1.SetActive(true);
        acto_1_playable.Play();

        playerr.SetActive(false);
        brazoPlayer.SetActive(false);

        for (int i = 0; i < UIvisuals.Length; i++)
        {
            UIvisuals[i].SetActive(false);
        }

        GameObject backroomNoise = GameObject.FindGameObjectWithTag("BackroomNoise");
        if (backroomNoise != null)
            Destroy(backroomNoise);


        // Buscar objetos necesarios (opcional)
        GameObject puertaBackroom = GameObject.FindGameObjectWithTag("puertaBackroom");
        if (puertaBackroom != null)
            Destroy(puertaBackroom);
    }

    private void OnActo1Terminado(PlayableDirector director)
    {
        // Quitar la suscripción para evitar llamadas duplicadas
        acto_1_playable.stopped -= OnActo1Terminado;

        // Llamar al siguiente paso
        TerminaActo1();
    }

    public void TerminaActo1()
    {
        acto_1.SetActive(false);
        acto_1_playable.Stop();

        // Vincular evento del acto 2 también
        acto_2_playable.stopped += OnActo2Terminado;

        acto_2.SetActive(true);
        acto_2_playable.Play();

        GameObject backrooms = GameObject.FindGameObjectWithTag("Backrooms");
        if (backrooms != null)
            Destroy(backrooms);
    }

    private void OnActo2Terminado(PlayableDirector director)
    {
        acto_2_playable.stopped -= OnActo2Terminado;
        TerminaActo2();
    }

    public void TerminaActo2()
    {
        SceneManager.LoadScene("Creditos");
    }
}
