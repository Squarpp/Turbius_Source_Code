using System.Collections;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject loadingCanv;
    public void Reintentar()
    {
        StartCoroutine(loadingScene("InGame"));
    }
    public void Menu()
    {
        StartCoroutine(loadingScene("Menu"));
    }

    public void MouseVisible()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator loadingScene(string sceneLoad)
    {
        loadingCanv.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneLoad);
    }
}
