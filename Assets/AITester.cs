using UnityEngine;
using UnityEngine.AI;

public class AITester : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent no encontrado en " + gameObject.name);
            return;
        }

        if (player == null)
        {
            Debug.LogError("No se ha asignado un jugador al AITester.");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("El agente no está en un NavMesh.");
        }

        // Ajusta la distancia de parada para que se acerque más al jugador.
        agent.stoppingDistance = 0f;
    }

    void Update()
    {
        if (agent != null && player != null && agent.isOnNavMesh)
        {
            // Actualiza el destino del agente
            agent.SetDestination(player.position);
        }
    }
}
