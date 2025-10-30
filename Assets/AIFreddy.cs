using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class AIFreddy : MonoBehaviour
{
    [Header("Referencias")]
    private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    [Header("Recorridos (listas de puntos)")]
    [SerializeField] private Transform[] recorrido1;
    [SerializeField] private Transform[] recorrido2;

    private Transform[][] recorridos; // matriz de recorridos
    private int recorridoActual = 0;  // cuál recorrido está siguiendo
    private int puntoActual = 0;      // punto dentro del recorrido

    [Header("Config")]
    [SerializeField] private float distanciaMinima = 0.5f; // tolerancia de llegada
    [SerializeField] private float tiempoQuieto = 3f;      // cuánto tiempo queda quieto entre recorridos

    private bool estaQuieto = false;
    private bool terminoTodos = false;

    [SerializeField] public float distanciaAnim;

    [SerializeField] public FieldOfView fov;
    [SerializeField] public Transform player;
    [SerializeField] public MultiAimConstraint headRig;
    [SerializeField] public bool chase;
    [SerializeField] public float speed; 
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSSpeed;
    [SerializeField] public bool scare;
    [SerializeField] public bool scareOneTime;
    [SerializeField] public GameObject Avoiders;

    private Coroutine followRoutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Transform>();
        recorridos = new Transform[][]
        {
            recorrido1,
            recorrido2
        };

        if (recorridos[recorridoActual].Length > 0)
        {
            MoverASiguientePunto();
        }
    }

    void Update()
    {
        if (!scare)
        {
            headRig.weight = chase ? 1 : 0;
            speed = chase ? runSSpeed : walkSpeed;
            agent.speed = speed;

            anim.SetFloat("Speed", speed);

            if (fov.canSeePlayer)
            {
                chase = true;
            }

            if (chase)
            {
                if (Avoiders != null)
                {
                    Avoiders.SetActive(false);
                }
                if (followRoutine != null)
                {
                    StopCoroutine(followRoutine);
                    followRoutine = null;
                }
                agent.SetDestination(player.position);
            }

            if (terminoTodos) return;

            if (!estaQuieto && !agent.pathPending && agent.remainingDistance <= distanciaMinima)
            {
                AvanzarPunto();
            }

            // Animaciones según velocidad
            if (agent.velocity.magnitude > distanciaAnim)
                anim.SetBool("isWalking", true);
            else
                anim.SetBool("isWalking", false);
        }
        else
        {
            if (!scareOneTime)
            {
                agent.speed = 0;
                anim.Play("jumpscare_Freddy", 0, 0);
                scareOneTime = true;
            }
        }

       
    }


    public void FollowSite(Transform poss)
    {
        if (poss == null) return;

        // detener cualquier follow anterior
        if (followRoutine != null)
        {
            StopCoroutine(followRoutine);
            followRoutine = null;
        }

        // arrancar la corutina que va al sitio y luego reanuda el patrullaje
        followRoutine = StartCoroutine(FollowPlaceAndResumePatrol(poss));
    }

    IEnumerator FollowPlaceAndResumePatrol(Transform target)
    {
        // si estaba en estado "quieto", forzamos la reanudación para que pueda moverse
        estaQuieto = false;
        agent.isStopped = false;

        // ir al objetivo
        agent.SetDestination(target.position);

        // esperar hasta llegar AL SITIO, o hasta que detecte al jugador (en cuyo caso abortamos y dejamos que Update() haga el chase)
        yield return new WaitUntil(() => (!agent.pathPending && agent.remainingDistance <= distanciaMinima) || (fov != null && fov.canSeePlayer));

        // si detectó al jugador, cancelamos este follow (Update manejará el chase)
        if (fov != null && fov.canSeePlayer)
        {
            followRoutine = null;
            yield break;
        }

        // pequeño descanso opcional en el sitio
        yield return new WaitForSeconds(0.1f);

        // reanudar patrullaje normal (si no terminó todos)
        if (!terminoTodos && recorridos != null && recorridos.Length > 0 && recorridos[recorridoActual].Length > 0)
        {
            // asegurarnos que puntoActual esté en rango
            if (puntoActual >= recorridos[recorridoActual].Length) puntoActual = 0;
            MoverASiguientePunto();
        }

        followRoutine = null;
    }



    void AvanzarPunto()
    {
        puntoActual++;

        // si terminó el recorrido actual
        if (puntoActual >= recorridos[recorridoActual].Length)
        {
            StartCoroutine(EsperarEntreRecorridos());
        }
        else
        {
            MoverASiguientePunto();
        }
    }

    IEnumerator EsperarEntreRecorridos()
    {
        estaQuieto = true;
        agent.isStopped = true;
        anim.SetBool("isWalking", false);

        yield return new WaitForSeconds(tiempoQuieto);

        recorridoActual++;

        if (recorridoActual >= recorridos.Length)
        {
            // ya hizo todos los recorridos → queda quieto para siempre
            terminoTodos = true;
            yield break;
        }

        puntoActual = 0;
        estaQuieto = false;
        agent.isStopped = false;
        MoverASiguientePunto();
    }

    void MoverASiguientePunto()
    {
        if (recorridos[recorridoActual].Length == 0) return;

        agent.SetDestination(recorridos[recorridoActual][puntoActual].position);
    }
}
