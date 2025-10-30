using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingInGame : MonoBehaviour
{
    public void loading()
    {
        SceneManager.LoadScene("InGame");
    }
}
