using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfiguracionesMenu : MonoBehaviour
{
    [Header("Audio")]
   public AudioMixer audioMixer; // Arrastra tu mixer
   public Slider volumenSlider;

    [Header("FPS")]
    public Slider fpsSlider;
    public TMP_Text fpsTexto;

    [Header("Resolución y pantalla")]
    public TMP_Dropdown resolucionDropdown;
    public Toggle pantallaCompletaToggle;
    public Toggle motionBlurToggle;

    [Header("Botones")]
    public Button aplicarBoton;
    public Button cancelarBoton;

    Resolution[] resoluciones;
    public bool _cambiosPendientes = false;

    // Variables temporales (para cambios no guardados)
    float _tempVolumen;
    int tempFPS;
    int tempResolucionIndex;
    bool tempPantallaCompleta;
    bool tempMotionBlur;

    // Claves de PlayerPrefs
    const string VOLUMEN_KEY = "Volumen";
    const string FPS_KEY = "FPS";
    const string RESOLUCION_KEY = "Resolucion";
    const string PANTALLA_COMPLETA_KEY = "PantallaCompleta";
    const string MOTION_BLUR_KEY = "MotionBlur";

    public GameObject visualConfig;


    void Start()
    {
        visualConfig.SetActive(false);
        resoluciones = Screen.resolutions;
        resolucionDropdown.ClearOptions();

        List<string> opciones = new List<string>();
        int indiceActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if (resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                indiceActual = i;
            }
        }

        resolucionDropdown.AddOptions(opciones);
        resolucionDropdown.value = PlayerPrefs.GetInt(RESOLUCION_KEY, indiceActual);
        resolucionDropdown.RefreshShownValue();


        StartCoroutine(InitSettings());
    }

    IEnumerator InitSettings()
    {
        yield return null; // Espera 1 frame para que el Mixer esté listo

        // Cargar configuraciones
        float vol = PlayerPrefs.GetFloat(VOLUMEN_KEY, 0.75f);
        int fps = PlayerPrefs.GetInt(FPS_KEY, 60);
        int resIndex = PlayerPrefs.GetInt(RESOLUCION_KEY, 0);
        bool pantallaCompleta = PlayerPrefs.GetInt(PANTALLA_COMPLETA_KEY, 1) == 1;
        bool motionBlur = PlayerPrefs.GetInt(MOTION_BLUR_KEY, 1) == 1;

        // Aplicar a slider y mixer
        volumenSlider.value = vol;
        SetMixerVolume(vol); // ← usamos la nueva función abajo

        _tempVolumen = vol;
        tempFPS = fps;
        tempResolucionIndex = resIndex;
        tempPantallaCompleta = pantallaCompleta;
        tempMotionBlur = motionBlur;

        fpsSlider.value = fps;
        ActualizarTextoFPS(fps);
        resolucionDropdown.value = resIndex;
        pantallaCompletaToggle.isOn = pantallaCompleta;
        motionBlurToggle.isOn = motionBlur;

        OnMotionBlurChange(motionBlur);

        // Eventos
        volumenSlider.onValueChanged.AddListener(OnVolumenChange);
        fpsSlider.onValueChanged.AddListener(OnFPSChange);
        resolucionDropdown.onValueChanged.AddListener(OnResolucionChange);
        pantallaCompletaToggle.onValueChanged.AddListener(OnPantallaCompletaChange);
        motionBlurToggle.onValueChanged.AddListener(OnMotionBlurChange);

        aplicarBoton.onClick.AddListener(AplicarConfiguraciones);
        cancelarBoton.onClick.AddListener(CancelarCambios);

        aplicarBoton.interactable = false;
        visualConfig.SetActive(true);
    }

    void OnVolumenChange(float value)
    {
        // Este se aplica y guarda al instante
        audioMixer.SetFloat("MasterVolume", value > 0 ? Mathf.Log10(value) * 20 : -80f);
        PlayerPrefs.SetFloat(VOLUMEN_KEY, value);
        PlayerPrefs.Save();
        _tempVolumen = value;
    }
    public void Volver()
    {
        Destroy(gameObject);
    }
    void SetMixerVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", value > 0 ? Mathf.Log10(value) * 20 : -80f);
    }


    void OnFPSChange(float value)
    {
        tempFPS = Mathf.RoundToInt(value);
        ActualizarTextoFPS(tempFPS);
        _cambiosPendientes = true;
        aplicarBoton.interactable = true;
    }

    void ActualizarTextoFPS(int fps)
    {
        if (fps <= 0)
            fpsTexto.text = "Ilimitado";
        else
            fpsTexto.text = fps.ToString();
    }

    void OnResolucionChange(int index)
    {
        tempResolucionIndex = index;
        _cambiosPendientes = true;
        aplicarBoton.interactable = true;
    }

    void OnPantallaCompletaChange(bool estado)
    {
        tempPantallaCompleta = estado;
        _cambiosPendientes = true;
        aplicarBoton.interactable = true;
    }

    void OnMotionBlurChange(bool estado)
    {
        tempMotionBlur = estado;
        PlayerPrefs.SetInt(MOTION_BLUR_KEY, tempMotionBlur ? 1 : 0);
        PlayerPrefs.Save();
    }

    void AplicarConfiguraciones()
    {
        // FPS
        if (tempFPS <= 0)
            Application.targetFrameRate = -1; // Ilimitado
        else
            Application.targetFrameRate = tempFPS;

        // Resolución
        Resolution res = resoluciones[tempResolucionIndex];
        Screen.SetResolution(res.width, res.height, tempPantallaCompleta);

        // Guardar
        PlayerPrefs.SetInt(FPS_KEY, tempFPS);
        PlayerPrefs.SetInt(RESOLUCION_KEY, tempResolucionIndex);
        PlayerPrefs.SetInt(PANTALLA_COMPLETA_KEY, tempPantallaCompleta ? 1 : 0);
        PlayerPrefs.Save();

        _cambiosPendientes = false;
        aplicarBoton.interactable = false;

        // 🔄 Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void CancelarCambios()
    {
        // Restaurar UI desde PlayerPrefs
        fpsSlider.value = PlayerPrefs.GetInt(FPS_KEY, 60);
        ActualizarTextoFPS((int)fpsSlider.value);

        resolucionDropdown.value = PlayerPrefs.GetInt(RESOLUCION_KEY, 0);
        pantallaCompletaToggle.isOn = PlayerPrefs.GetInt(PANTALLA_COMPLETA_KEY, 1) == 1;

        _cambiosPendientes = false;
        aplicarBoton.interactable = false;
    }
}
