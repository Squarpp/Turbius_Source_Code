using TMPro;
using UnityEngine;

public class PaperCode : MonoBehaviour
{
    public string pin;
    [SerializeField] private TextMeshPro tmp; // Usar UGUI si es texto de UI

    private void Start()
    {
        // Genera un número aleatorio entre 0 y 99999 inclusive
        int randomNumber = Random.Range(0, 100000);

        // Lo convierte a string de 5 dígitos, con ceros a la izquierda si hace falta
        pin = randomNumber.ToString("D5");

        // Lo muestra en pantalla
        tmp.text = pin;
    }
}
