using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovementGrid : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float tileSize = 0.1f;
    public LayerMask obstacleLayer;

    private bool isMoving = false;
    private Vector2 input;
    private Vector2 lastInput;

    public Animator animator;

    public GameObject dirtPrefab;
    public GameObject rockBreakEffect; 
    public GameObject rock;
    public Transform respawn;

    [SerializeField] private AudioSource asrcMusic;
    [SerializeField] private AudioSource asrc;

    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip clipPush;

    void Start()
    {
        animator.SetBool("isMoving", false);
        animator.speed = 0f;
        animator.Play("Walk", 0, 0f);
    }

    void OnEnable()
    {
        asrcMusic.clip = music;
        asrcMusic.Play();
    }


    void Update()
    {
        if (!isMoving)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                lastInput = input;
                Vector3 targetPos = transform.position + new Vector3(input.x * tileSize, input.y * tileSize, 0);

                // Verificamos si hay una roca (Pushable)
                Collider2D pushable = Physics2D.OverlapCircle(targetPos, tileSize / 4f);
                if (pushable != null && pushable.CompareTag("Pushable"))
                {
                    Vector3 rockTarget = pushable.transform.position + new Vector3(input.x * tileSize, input.y * tileSize, 0);

                    if (IsWalkable(rockTarget))
                    {
                        UpdateAnimator();
                        StartCoroutine(MoveRock(pushable.gameObject, rockTarget));
                    }
                    else
                    {
                        // Si no se puede mover la roca, se destruye
                        Instantiate(rockBreakEffect, pushable.transform.position, Quaternion.identity); //instanciando la wea
                        StartCoroutine(respawningRock());
                        Destroy(pushable.gameObject);
                    }

                    // No mover al jugador en este caso
                    return;
                }

                // Movimiento normal si el tile está libre
                if (IsWalkable(targetPos))
                {
                    UpdateAnimator();
                    StartCoroutine(Move(targetPos));
                }
            }
            else
            {
                animator.SetBool("isMoving", false);
                animator.speed = 0f;
                animator.Play("Walk", 0, 0f);
            }
        }
    }

    bool IsWalkable(Vector3 targetPos)
    {
        float radius = tileSize / 16f;
        Collider2D hit = Physics2D.OverlapCircle(targetPos, radius, obstacleLayer);
        return hit == null;
    }

    System.Collections.IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        Vector3 startPos = transform.position;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    System.Collections.IEnumerator MoveRock(GameObject rock, Vector3 targetPos)
    {
        asrc.PlayOneShot(clipPush);
        isMoving = true;
        Vector3 startPos = rock.transform.position;

        if (dirtPrefab != null)
        {
            Instantiate(dirtPrefab, startPos, Quaternion.identity);
        }

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            rock.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        rock.transform.position = targetPos;
        isMoving = false;
    }

    IEnumerator respawningRock()
    {
        yield return new WaitForSeconds(5);
        Instantiate(rock, respawn.transform.position, Quaternion.identity);
    }

    void UpdateAnimator()
    {
        animator.speed = 1f;
        animator.SetFloat("MoveX", input.x);
        animator.SetFloat("MoveY", input.y);
        animator.SetBool("isMoving", true);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 checkPos = transform.position + new Vector3(input.x * tileSize, input.y * tileSize, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(checkPos, tileSize / 2.5f);
    }
}
