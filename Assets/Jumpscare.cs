using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas;

    private bool triggered = false; // para evitar múltiples triggers

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return; // evita que se ejecute más de una vez

        Debug.Log($"Trigger detectado con: {other.name}, tag: {other.tag}");

        if (other.CompareTag("enemy"))
        {
            EnemyAI enemyAI = other.GetComponentInParent<EnemyAI>();
            if (enemyAI != null)
            {
                Debug.Log($"EnemyAI detectado en {other.name}. Activando jumpscare...");
                enemyAI.scare = true;
                triggered = true;
                DestroyCanvas();
            }
            else
            {
                Debug.LogWarning($"El objeto con tag 'enemy' ({other.name}) NO tiene componente EnemyAI.");
            }
        }

        if (other.CompareTag("enemyFreddy"))
        {
            AIFreddy freddyAI = other.GetComponent<AIFreddy>();
            if (freddyAI != null)
            {
                Debug.Log($"AIFreddy detectado en {other.name}. Activando jumpscare...");
                freddyAI.scare = true;
                triggered = true;
                DestroyCanvas();
            }
            else
            {
                Debug.LogWarning($"El objeto con tag 'enemyFreddy' ({other.name}) NO tiene componente AIFreddy.");
            }
        }
    }

    void DestroyCanvas()
    {
        if (canvas != null)
        {
            Destroy(canvas);
            Debug.Log("Canvas destruido.");
        }
        else
        {
            Debug.LogWarning("Canvas no asignado.");
        }

        if (player != null)
        {
            Destroy(player);
            Debug.Log("Jugador destruido.");
        }
        else
        {
            Debug.LogWarning("Jugador no asignado.");
        }
    }
}
