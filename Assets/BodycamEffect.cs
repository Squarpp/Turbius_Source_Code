using UnityEngine;
using VHS;

public class BodycamEffect : MonoBehaviour
{
    [Header("Movimiento")]
    public float movementIntensity = 0.01f;
    public float movementSpeed = 10f;

    [Header("Rotación")]
    public float rotationIntensity = 0.2f;
    public float rotationSpeed = 5f;

    [SerializeField] private float potencial = 1f;
    public VHS.FirstPersonController fps;

    [Header("Ruido")]
    public float noiseScale = 1f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public float timeOffset;

    // Base
    private float baseMovementIntensity;
    private float baseRotationIntensity;

    // Shake temporal
    private float shakeMovementExtra = 0f;
    private float shakeRotationExtra = 0f;
    private float shakeTimer = 0f;

    public float ActualRunningPotencial = 1.5f; // ejemplo

    [SerializeField] private MovementInputData movementInputData = null;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        baseMovementIntensity = movementIntensity;
        baseRotationIntensity = rotationIntensity;
    }

    void Update()
    {
        // --- Cálculo del potencial con suavizado ---
        float targetPotencial = 1f;
        if (fps != null)
        {
            if (fps.isWalking)
                targetPotencial = 2f;
        }

        if (movementInputData.IsRunning && !fps.IsCrouching)
            targetPotencial *= ActualRunningPotencial;

        // Suavizar el cambio (no brusco)
        potencial = Mathf.Lerp(potencial, targetPotencial, Time.deltaTime * 5f);

        // --- Shake temporal ---
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                shakeMovementExtra = 0f;
                shakeRotationExtra = 0f;
                shakeTimer = 0f;
            }
        }
        else
        {
            shakeMovementExtra = Mathf.Lerp(shakeMovementExtra, 0f, Time.deltaTime * 5f);
            shakeRotationExtra = Mathf.Lerp(shakeRotationExtra, 0f, Time.deltaTime * 5f);
        }

        // --- Movimiento / Rotación tipo ruido ---
        // NO multiplicamos el tiempo por potencial → evita tirones
        float time = (Time.time * noiseScale) + timeOffset;

        // Movimiento tipo “temblor”
        Vector3 noisePos = new Vector3(
            Mathf.PerlinNoise(time, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, time) - 0.5f,
            Mathf.PerlinNoise(time, time) - 0.5f
        );

        // Aplicar suavemente el movimiento (amplitud depende del potencial)
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            initialPosition + noisePos * (baseMovementIntensity + shakeMovementExtra) * potencial,
            Time.deltaTime * movementSpeed
        );

        // Rotación leve tipo “shaky cam”
        Vector3 noiseRot = new Vector3(
            Mathf.PerlinNoise(time + 10f, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, time + 10f) - 0.5f,
            Mathf.PerlinNoise(time + 20f, time + 20f) - 0.5f
        );

        float currentRotationIntensity = (baseRotationIntensity + shakeRotationExtra) * potencial;
        Quaternion targetRot = Quaternion.Euler(noiseRot * currentRotationIntensity * 10f);

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            initialRotation * targetRot,
            Time.deltaTime * (rotationSpeed)
        );
    }

    // --- Shake puntual ---
    public void ShakeOnce(float extraMovement, float extraRotation, float duration)
    {
        shakeMovementExtra = extraMovement;
        shakeRotationExtra = extraRotation;
        shakeTimer = duration;
    }
}
