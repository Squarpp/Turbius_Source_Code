using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreditsBPM : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI tmpText;

    [Header("Credits")]
    [TextArea(1, 10)]
    public string[] credits;

    [Header("Beat Settings")]
    [Min(1)] public float bpm = 120f;     // beats por minuto
    [Tooltip("Offset en segundos. Positivo = empieza más tarde. Negativo = empieza antes del primer beat.")]
    public float offset = 0f;             // puede ser negativo
    public bool autoStart = true;
    public bool loop = false;

    [Header("Fade Settings")]
    public bool fadeTransition = true;
    public float fadeDuration = 0.3f;

    int index = 0;
    Coroutine routine;


    private void Update()
    {
        if (index > credits.Length - 1 || Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void Start()
    {
        if (tmpText == null)
            tmpText = GetComponent<TextMeshProUGUI>();

        if (autoStart)
            StartCredits();
    }

    public void StartCredits()
    {
        StopCredits();
        index = 0;
        routine = StartCoroutine(RunCredits());
    }

    public void StopCredits()
    {
        if (routine != null) StopCoroutine(routine);
        routine = null;
    }

    IEnumerator RunCredits()
    {
        if (credits.Length == 0)
            yield break;

        float beatInterval = 60f / bpm; // segundos por beat

        // si offset es negativo, adelantamos el índice o el primer cambio
        float currentTime = offset; // puede empezar negativo

        while (true)
        {
            if (currentTime >= 0f)
            {
                // Mostrar texto actual
                string line = credits[index];
                if (fadeTransition)
                    yield return StartCoroutine(FadeText(line));
                else
                    tmpText.text = line;

                // Esperar hasta el próximo beat
                yield return new WaitForSeconds(beatInterval);
                index++;
                if (index >= credits.Length)
                {
                    if (loop)
                        index = 0;
                    else
                        yield break;
                }
            }
            else
            {
                // saltar tiempo negativo sin mostrar nada (simula “antes” del beat)
                yield return new WaitForSeconds(Mathf.Abs(currentTime));
                currentTime = 0f;
            }
        }
    }

    IEnumerator FadeText(string newText)
    {
        float t = 0f;
        Color c = tmpText.color;

        // fade out
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            tmpText.color = new Color(c.r, c.g, c.b, Mathf.Lerp(1, 0, t / fadeDuration));
            yield return null;
        }

        tmpText.text = newText;

        // fade in
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            tmpText.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0, 1, t / fadeDuration));
            yield return null;
        }

        tmpText.color = new Color(c.r, c.g, c.b, 1);
    }
}
