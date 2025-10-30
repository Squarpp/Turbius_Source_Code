using UnityEngine;
using System.Collections;

public class LimpiezaDeMemoria : MonoBehaviour
{
    void Start()
    {
        // Llama la limpieza de assets no usados
        StartCoroutine(LimpiarMemoria());
    }

    IEnumerator LimpiarMemoria()
    {
        // Espera un frame antes de limpiar (opcional, pero recomendado)
        yield return null;

        // Limpia assets que ya no están referenciados
        AsyncOperation async = Resources.UnloadUnusedAssets();

        // Espera a que termine
        yield return async;

        Debug.Log("Limpieza de memoria completada.");
    }
}
