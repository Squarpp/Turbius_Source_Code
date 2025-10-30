using UnityEngine;
using VHS;

public class SoundEffects : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioSource asource; 
    [SerializeField] private AudioSource asourceAmbience;

    [SerializeField] public VHS.FirstPersonController fps;

    [Header("Audio Clips")]
    [Tooltip("Sonidos de caminata del personaje")]
    [SerializeField] private AudioClip[] caminataMadera;
    [SerializeField] private AudioClip[] caminataPasto;
    [SerializeField] private AudioClip[] caminataVent;
    [SerializeField] private AudioClip[] caminataBackrooms;
    [SerializeField] private AudioClip[] caminataConcrete;


    [SerializeField] private AudioClip[] ambience;

    [Header("Configuración de Sonido")]
    [Tooltip("Tiempo mínimo entre pasos")]
    [SerializeField] private float pasoDelay = 0.4f;

    [Space, Header("Data")]
    [SerializeField] private MovementInputData movementInputData = null;

    private float tiempoUltimoPaso;

    [SerializeField] public string getString;

    private void Start()
    {
        tiempoUltimoPaso = -pasoDelay;

        asourceAmbience.clip = ambience[0];
        asourceAmbience.Play();
    }

    private void Update()
    {
        bool runnin = movementInputData.IsRunning;
        float multiplyDelay = runnin ? 0.6f : 1;
        float crouchingMultiply = fps.IsCrouching ? 1.5f : 1f;

        if (fps.IsCrouching)
        {
            multiplyDelay = 1;
        }

        if (fps.isWalking)
        {
            if (fps != null && Time.time - tiempoUltimoPaso >= pasoDelay * multiplyDelay * crouchingMultiply)
            {
                Caminata(getString);
                tiempoUltimoPaso = Time.time;
            }
        }
    }

    private void Caminata(string strin)
    {
        if (caminataMadera.Length == 0) return;
        if (caminataPasto.Length == 0) return;

        int indexMadera = Random.Range(0, caminataMadera.Length);
        int indexPasto = Random.Range(0, caminataPasto.Length);
        int indexVent = Random.Range(0, caminataVent.Length);
        int indexBackroom = Random.Range(0, caminataBackrooms.Length);
        int indexConcrete = Random.Range(0, caminataConcrete.Length);

        switch (strin)
        {
            case "woodfloor":
                asource.PlayOneShot(caminataMadera[indexMadera]);
                break;
            case "pasto":
                asource.PlayOneShot(caminataPasto[indexPasto]);
                break;
            case "vent":
                asource.PlayOneShot(caminataVent[indexVent]);
                break;
            case "backroomfloor":
                asource.PlayOneShot(caminataBackrooms[indexBackroom]);
                break;
            case "concrete":
                asource.PlayOneShot(caminataConcrete[indexConcrete]);
                break;
        }
    }
}
