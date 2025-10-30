using UnityEngine;

public class TurbiusSlower : MonoBehaviour
{
    [Header("Referencias")]
    public Flashlight flashlight;

    [Header("Configuración del Raycast")]
    public float rayLength = 10f;
    public float rayRadius = 0.5f;
    public LayerMask detectionMask;

    [Header("Estabilidad")]
    public float exitDelay = 0.3f; // segundos que tarda en "apagar" el enemigo tras dejar de verlo

    private EnemyAI lastDetectedEnemy;
    private float lastSeenTime;

    void Update()
    {
        DetectEnemies();
    }

    void DetectEnemies()
    {
        Vector3 direction = transform.forward;

        Debug.DrawRay(transform.position, direction * rayLength, Color.yellow);

        if (flashlight == null)
        {
            Debug.LogWarning("[TurbiusSlower] Falta asignar el componente Flashlight.");
            return;
        }

        bool hitEnemyThisFrame = false;

        // Raycast tipo esfera
        if (Physics.SphereCast(transform.position, rayRadius, direction, out RaycastHit hit, rayLength, detectionMask))
        {
            EnemyAI enemy = hit.collider.GetComponentInParent<EnemyAI>();

            if (enemy != null)
            {
                hitEnemyThisFrame = true;
                lastSeenTime = Time.time; // reinicia temporizador

                if (flashlight.on)
                {
                    if (!enemy.isFlashlighted)
                        Debug.Log("[TurbiusSlower] → Linterna encendida, iluminando: " + enemy.name);

                    enemy.isFlashlighted = true;
                    lastDetectedEnemy = enemy;
                }
            }
        }

        // Si no lo detectó este frame, verificamos si pasó suficiente tiempo sin verlo
        if (!hitEnemyThisFrame && lastDetectedEnemy != null)
        {
            if (Time.time - lastSeenTime > exitDelay)
            {
                if (lastDetectedEnemy.isFlashlighted)
                    Debug.Log("[TurbiusSlower] → Enemigo salió del haz, desmarcando: " + lastDetectedEnemy.name);

                lastDetectedEnemy.isFlashlighted = false;
                lastDetectedEnemy = null;
            }
        }
    }
}
