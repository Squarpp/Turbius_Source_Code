using UnityEngine;

public class DetectarMirada : MonoBehaviour
{
    public Camera camara;
    public float anguloDeteccion = 15f; // cuán precisa debe ser la mirada
    public float distanciaMaxima = 20f;
    public bool ifSonido;
    private bool sonidoUnaVez; 
    [SerializeField] private float sonidoTimer;

    [SerializeField] private AudioSource asrc;
    [SerializeField] private AudioClip voz_rubius;
    public GameObject Flashlight;

    void Update()
    {
        if(camara != null){

        // Vector hacia el objeto
        Vector3 direccionHaciaObjeto = transform.position - camara.transform.position;

        // Ángulo entre la dirección de la cámara y el objeto
        float angulo = Vector3.Angle(camara.transform.forward, direccionHaciaObjeto);

        // Si está dentro del campo visual y no demasiado lejos
        if (angulo < anguloDeteccion && direccionHaciaObjeto.magnitude < distanciaMaxima)
        {
                if (Flashlight.activeSelf) sonidoTimer += Time.deltaTime;

            }
        else
        {
                sonidoTimer = 0;
        }

        if(sonidoTimer > 1 && ifSonido && !sonidoUnaVez)
        {
            asrc.PlayOneShot(voz_rubius);
                sonidoUnaVez = true;
                sonidoTimer = 0;
        }
        }
    }
}
