using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    public FieldOfView view;
    public Transform player;
    public bool Chase;
    public Transform[] Positions; // Array con las 4 posiciones
    private int currentIndex = 0; // Índice de la posición actual
    public GameObject Avoiders;
    public int fase; 
    public float timeChase;
    public VHS.FirstPersonController firstPersonController;
    private bool oneTime;
    public bool scare;
    public VideoPlayer video;
    public VideoClip chernobyl;
    public float LastRun;
    public float NormalRun;
    [SerializeField] private AudioSource audioSourceSong;
    [SerializeField] private AudioSource audioSourceAI;
    [SerializeField] private AudioClip clipChase;
    [SerializeField] private AudioClip clipChaseAI;
    private bool chaseOneTime;
    public float FlashlightSpeed = 1;
    public bool isFlashlighted;
    public float FlashlightTime;
    public LookAtPlayerHead cabezaLook;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (cabezaLook != null) cabezaLook = GetComponentInChildren<LookAtPlayerHead>();
        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        if (video == null) video = GetComponent<VideoPlayer>();
        if (firstPersonController == null)
            firstPersonController = FindAnyObjectByType<VHS.FirstPersonController>();

        if (audioSourceSong == null)
            audioSourceSong = GameObject.FindGameObjectWithTag("AudioSourceSong")?.GetComponent<AudioSource>();

        if (!Chase)
        {
            animator.SetBool("chase", false);
            StartCoroutine(FollowASite());
        }

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

        agent.stoppingDistance = 0f;
    }


    void Update()
    {
        if (!scare)
        {
            IfNotJumpscareLol();
        }
        else
        {
            if(cabezaLook != null) cabezaLook.miraAlJugador = false;
            animator.speed = 1f;
            animator.SetBool("jumpscare", true);
            Destroy(agent);
        }

        FlashlightSpeed = isFlashlighted ? 0.5f : 1.2f;
        animator.SetBool("light", isFlashlighted);
        if (isFlashlighted) FlashlightTime += Time.deltaTime;

        if (isFlashlighted && FlashlightTime > 2.8f) Chase = true;
        if (!isFlashlighted) FlashlightTime -= Time.deltaTime*2;

        if (FlashlightTime < 0) FlashlightTime = 0;
    }

    void IfNotJumpscareLol()
    {
        if (cabezaLook != null && !isFlashlighted) {
            cabezaLook.miraAlJugador = Chase;
        }
        else
        {
            cabezaLook.miraAlJugador = false;
        }

        // Si el enemigo está siguiendo al jugador
        if (Chase)
        {
            if (!chaseOneTime)
            {
                audioSourceSong.PlayOneShot(clipChase);
                audioSourceAI.clip = clipChaseAI;
                audioSourceAI.Play();
                chaseOneTime = true;
            }


            if (timeChase > 8.6f)
            {
                fase++;
                timeChase = 0f;
            }

            // Control de velocidad y fases
            switch (fase)
            {
                case 0:
                    agent.speed = NormalRun * FlashlightSpeed;
                    animator.speed = NormalRun / 2;
                    break;
                case 2:
                    agent.angularSpeed = 900000;
                    agent.speed = LastRun * FlashlightSpeed;
                    animator.speed = (LastRun / 3) * FlashlightSpeed;
                    break;
                case 3:
                    agent.angularSpeed = 900000;
                    agent.speed = LastRun * FlashlightSpeed * 1.1f;
                    animator.speed = (LastRun / 3) * FlashlightSpeed * 1.1f;
                    break;
            }

            if (fase > 4)
            {
                firstPersonController.chase = false;
                gameObject.SetActive(false);
            }

            if (!oneTime)
            {
                firstPersonController.chase = true;
                if (video != null)
                {
                    video.clip = chernobyl;
                    video.Play();
                }
                oneTime = true;
            }

            // Intentar seguir al jugador
            bool canFollow = FollowPlayer();

            // Solo animar si realmente puede moverse hacia el jugador
            animator.SetBool("walk", canFollow);
            animator.SetBool("chase", canFollow);

            timeChase += Time.deltaTime;
        }
        else
        {
            animator.speed = 1f;
            agent.speed = 1f;
            timeChase = 0f;
            // fase = 0; // si querés mantenerlo, ok, pero puede resetear demasiado pronto

            // Control de animación al patrullar
            bool isWalking = agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance;
            animator.SetBool("walk", isWalking);
            animator.SetBool("chase", false);
        }

        if (view.canSeePlayer)
        {
            Chase = true;
        }
    }



    public void FollowSite(Transform poss)
    {
        StopCoroutine(FollowASite());
        StartCoroutine(FollowAPlace(poss));
    }


    IEnumerator FollowASite()
    {
        while (true)
        {
            // Mover al agente a la posición actual en el array
            agent.SetDestination(Positions[currentIndex].position);

            yield return new WaitUntil(() => agent.remainingDistance <= 0.1f && !agent.pathPending);

            // Avanzar al siguiente punto (reseteando si es el último)
            currentIndex = (currentIndex + 1) % Positions.Length;
        }
    }
    IEnumerator FollowAPlace(Transform positionn)
    {
        while (true)
        {
            // Mover al agente a la posición actual en el array
            agent.SetDestination(positionn.position);

            yield return new WaitUntil(() => agent.remainingDistance <= 0.1f && !agent.pathPending);

            StartCoroutine(FollowASite());
        }
    }



    bool FollowPlayer()
    {
        if (agent != null && player != null && agent.isOnNavMesh)
        {
            NavMeshHit hit;
            bool playerOnNavMesh = NavMesh.SamplePosition(player.position, out hit, 1.5f, NavMesh.AllAreas);

            if (playerOnNavMesh)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                // Si el camino es válido y el agente se está moviendo realmente
                bool validPath = agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance > agent.stoppingDistance;
                return validPath;
            }
            else
            {
                agent.isStopped = true;
                agent.ResetPath();
                return false;
            }
        }
        return false;
    }



}
