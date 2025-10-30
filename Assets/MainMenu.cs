using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject transicionInGame;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject black;
    [SerializeField] private GameObject configuraciones;
    [SerializeField] private GameObject ibai;
    [SerializeField] private GameObject botonObjDiscord;
    [SerializeField] private Transform parentConfig;
    [SerializeField] private Transform parentConfigHide;

    private string discordLink = "https://discord.gg/dxcKtn2K"; // link por defecto

    void Start()
    {
        StartCoroutine(LoadDiscordLink());

        ibai.SetActive(PlayerPrefs.GetInt("win", 0) == 1);

    }
    private void Awake()
    {
        StartCoroutine(configStart());
    }

    IEnumerator configStart()
    {
        yield return new WaitForEndOfFrame();
        GameObject configNew = Instantiate(configuraciones, parentConfigHide);
        yield return new WaitForSeconds(0.1f);
        Destroy(configNew);
    }


    IEnumerator LoadDiscordLink()
    {
        string url = "https://squarpp.github.io/VSODiscordLink/discord.json";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            try
            {
                DiscordData data = JsonUtility.FromJson<DiscordData>(www.downloadHandler.text);
                if (!string.IsNullOrEmpty(data.discordLink))
                    discordLink = data.discordLink;
            }
            catch
            {
                Debug.LogWarning("Error leyendo JSON del repo");
            }
        }
        else
        {
            Debug.LogWarning("No se pudo descargar el JSON (sin internet o repo caído)");
        }

          if(botonObjDiscord != null && discordLink == "")
        {
            Destroy(botonObjDiscord);
        }
    }

    [System.Serializable]
    public class DiscordData
    {
        public string discordLink;
    }
    public void Continuar()
    {
        Instantiate(transicionInGame, canvas);
    }

    public void Discord()
    {
        if(discordLink != "")
        {
            Application.OpenURL(discordLink);
        }
    }
    public void Configuraciones()
    {
        Instantiate(configuraciones, parentConfig);
    }

    public void NuevaPartida()
    {
        // Guardar los datos que NO querés borrar
        int win = PlayerPrefs.GetInt("win", 0);

        // Borrar todo
        PlayerPrefs.DeleteAll();

        // Restaurar los datos que querés conservar
        PlayerPrefs.SetInt("win", win);

        // Aplicar los cambios
        PlayerPrefs.Save();

        // Instanciar la transición
        Instantiate(transicionInGame, canvas);
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void DestroyBlack()
    {
        Destroy(black);
    }
}
