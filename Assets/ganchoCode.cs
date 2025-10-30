using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ClawController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody rb;
    [SerializeField] public bool activated;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Asegura que lo controle la física
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY; // opcional: no rotar ni subir/bajar
    }

    void FixedUpdate()
    {
        if (activated)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z); // mantiene la gravedad si la hay
        }
    }
}
