using UnityEngine;

public class FixLag : MonoBehaviour
{
    public GameObject mountain; // Objeto a activar/desactivar
    private int triggerCount = 0; // Contador de colisiones activas

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerCount == 0) // Solo activar la primera vez
            {
                mountain.SetActive(true);
            }
            triggerCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerCount--;
            if (triggerCount == 0) // Solo desactivar cuando haya salido completamente
            {
                mountain.SetActive(false);
            }
        }
    }
}
