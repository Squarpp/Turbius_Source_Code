using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimDestroy : MonoBehaviour
{
    public AudioSource asource;
    public AudioClip scareclip;
    public bool turbiusMiniJumpscare;
    public bool spiderMiniJumpscare;
    public bool shrekMiniJumpscare;
    public void SoundScare()
    {
        asource.PlayOneShot(scareclip);

        if(turbiusMiniJumpscare)
        {
            PlayerPrefs.SetInt("turbiusMiniJumpscare", 1);
        }

        if (spiderMiniJumpscare)
        {
            PlayerPrefs.SetInt("spiderMiniJumpscare", 1);
        }

        if (shrekMiniJumpscare)
        {
            PlayerPrefs.SetInt("shrekMiniJumpscare", 1);
        }
    }
    public void Destroy()
    {
       Destroy(gameObject);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
