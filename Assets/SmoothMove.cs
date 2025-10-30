using UnityEngine;

public class SmoothMove : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // La referencia de la c�mara
    [SerializeField] private Transform TransformForMove; // El transform que se utiliza para la posici�n
    [SerializeField] private float smoothSpeed = 5f; // Velocidad del movimiento suave
    [SerializeField] private Vector3 positionOffset; // Offset para la posici�n
    private Quaternion targetRotation;
    public Vector3 targetRotationOffset;
    private Vector3 targetPosition;
    public float smoothY = 1;
    public float smoothX = 1;
    public float smoothZ = 1;

    void Update()
    {
        if(targetRotation != null) targetRotation = cameraTransform.rotation * Quaternion.Euler(targetRotationOffset);
        // Obtener la rotaci�n objetivo de la c�mara
        if (targetRotation != null) transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        // Interpolar suavemente hacia la rotaci�n objetivo
    }
      private void LateUpdate()
      {
        // Obtener la posici�n objetivo de TransformForMove con el offset
        if (targetPosition != null) targetPosition = TransformForMove.position + positionOffset;

                  // Interpolar suavemente solo el eje Y, pero mantener X y Z est�ticos
                     transform.position = new Vector3(
                         Mathf.Lerp(transform.position.x, targetPosition.x, smoothX * Time.deltaTime),
                         Mathf.Lerp(transform.position.y, targetPosition.y, smoothY * Time.deltaTime),
                         Mathf.Lerp(transform.position.z, targetPosition.z, smoothZ * Time.deltaTime)
                     );

    }  
}
