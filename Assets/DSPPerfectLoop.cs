using UnityEngine;

public class OverlapLoop : MonoBehaviour
{
    public AudioSource sourceA;
    public AudioSource sourceB;
    public float loopLength = 30f;    // duración total del loop en segundos
    public float overlapTime = 3f;    // segundos antes del final en que arranca el próximo

    private AudioSource currentSource;
    private AudioSource nextSource;
    private double nextStartTime;

    void Start()
    {
        currentSource = sourceA;
        nextSource = sourceB;

        double startTime = AudioSettings.dspTime + 0.1;
        currentSource.PlayScheduled(startTime);

        nextStartTime = startTime + (loopLength - overlapTime);
    }

    void Update()
    {
        if (AudioSettings.dspTime + 0.05 >= nextStartTime)
        {
            // Iniciar el siguiente audio solapado
            nextSource.PlayScheduled(nextStartTime);

            // Preparar la siguiente ronda
            AudioSource temp = currentSource;
            currentSource = nextSource;
            nextSource = temp;

            nextStartTime += (loopLength - overlapTime);
        }
    }
}
