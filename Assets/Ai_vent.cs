using UnityEngine;
using UnityEngine.AI;
using VHS;

public class Ai_vent : MonoBehaviour
{
    public Transform destination;  // Asignar en inspector
    public GameObject aiObj;       // Objeto opcional para desactivar
    public EnemyAI ai;        // Tu script de IA (tiene la variable Chase)

    private NavMeshAgent agent;
    public float arriveThreshold; // tolerancia de llegada

    public FirstPersonController fps;
    [SerializeField] private AudioSource asrc_ChaseSong;
    [SerializeField] private AudioSource asrc_Player;
    [SerializeField] private AudioClip scare;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (destination != null)
        {
            agent.SetDestination(destination.position);
        }
    }

    void Update()
    {
        if (destination == null) return;

        // 🔹 Cálculo vectorial en lugar de agent.remainingDistance
        float distance = Vector3.Distance(transform.position, destination.position);

        if (distance <= arriveThreshold)
        {
            asrc_Player.PlayOneShot(scare);
            asrc_ChaseSong.Stop();
            if (ai != null) ai.Chase = false;
            fps.chase = false;
            Destroy(aiObj);
            Destroy(gameObject);
        }
    }
}
