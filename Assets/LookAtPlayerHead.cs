using UnityEngine;

public class LookAtPlayerHead : MonoBehaviour
{
    [Header("Configuración de seguimiento")]
    public bool miraAlJugador = true; // 🔹 Si es false, no mira al jugador
    public Vector3 rotationOffset = new Vector3(0f, 0f, 0f); // Offset de rotación (en grados)

    [SerializeField] private Transform playerCam;

    void Start()
    {
        // Busca la cámara con tag "MainCamera" automáticamente
        if (playerCam == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
                playerCam = cam.transform;
            else
                Debug.LogWarning("No se encontró ninguna cámara con el tag 'MainCamera'. Asignala manualmente en el Inspector.");
        }
    }

    void LateUpdate()
    {
        if (!miraAlJugador || playerCam == null)
            return;

        // Mira directamente hacia la cámara
        transform.LookAt(playerCam);

        // Aplica un offset de rotación local (en grados)
        transform.Rotate(rotationOffset, Space.Self);
    }
}
