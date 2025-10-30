using TMPro;
using UnityEngine;
using System; // necesario para DateTime

public class HoraPause : MonoBehaviour
{
    private TextMeshProUGUI txmp;
    void Start()
    {
        // Obtenemos el componente TextMeshProUGUI
        txmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Mostramos la hora actual en formato 24hs (ejemplo: 21:34)
        txmp.text = DateTime.Now.ToString("HH:mm");
    }
}
