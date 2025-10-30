using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public float sensitivity = 2f; // Sensibilidad del ratón
    public Transform playerBody;   // Referencia al cuerpo del jugador
    private float xRotation = 0f;

    // Variables para el zoom
    public float zoomFOV = 30f;      // FOV al hacer zoom
    public float zoomSpeed = 5f;     // Velocidad del zoom
    private float defaultFOV;        // FOV original

    public Camera cam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro
        if (cam != null)
        {
            defaultFOV = cam.fieldOfView; // Guardar el FOV original
        }
    }

    void Update()
    {
        if (!PauseManager.isPaused)
        {
            // Rotación de la cámara
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevenir rotación excesiva

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX); // Rotar el cuerpo del jugador horizontalmente

            // Zoom suave con botón derecho del ratón
            if (cam != null)
            {
                if (Input.GetMouseButton(1)) // Si se mantiene presionado el botón derecho
                {
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomFOV, Time.deltaTime * zoomSpeed);
                }
                else
                {
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, Time.deltaTime * zoomSpeed);
                }
            }
        }
    }
}
