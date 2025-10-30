using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;      // Asigna el Canvas del menú de pausa
    public Button resumeButton;         // Asigna el botón para reanudar

    public static bool isPaused = false;
    public Button[] botones;
    public bool canPause = true;
    public int fixPause = 1;
    public GameObject ConfigObj;        // Prefab
    private GameObject currentConfig;   // Instancia actual en escena

    void Start()
    {
        ResumeGame();
        pauseMenuUI.SetActive(false); // Asegúrate de que el menú esté oculto al inicio
        resumeButton.onClick.AddListener(ResumeGame);
        foreach (Button boton in botones)
        {
            boton.onClick.AddListener(() => Deselect());
        }
    }

    void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && fixPause > 0)
            {
                if (isPaused) ResumeGame();
                else PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            fixPause++;
        }

    }

    public void BackToMenu()
    {
        ResumeGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }

    public void Config()
    {
        if (currentConfig == null)
            currentConfig = Instantiate(ConfigObj);
    }

    public void Restart()
    {
        ResumeGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("InGame");
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        AudioListener.pause = true; // 🔇 Pausa TODOS los sonidos

        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (currentConfig != null)
        {
            Destroy(currentConfig); // 🔥 Destruye la instancia en la escena
            currentConfig = null;
        }

        Time.timeScale = 1f;
        AudioListener.pause = false; // 🔊 Reanuda TODOS los sonidos

        isPaused = false;
    }

    void Deselect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
