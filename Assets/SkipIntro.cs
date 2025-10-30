using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipIntro : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool puede;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clipVSO;

    void Start()
    {
        int firstTime = PlayerPrefs.GetInt("ft", 0);

        if (firstTime == 0)
        {
            StartCoroutine(firstTimeCoroutine());
        }

        StartCoroutine(_puede());
    }

    IEnumerator firstTimeCoroutine()
    {
        PlayerPrefs.SetInt("ft", 1);
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene("InGame");
    }

    public void PlayAudioClipVso()
    {
        audioSource.PlayOneShot(clipVSO);
    }


    IEnumerator _puede()
    {
        yield return new WaitForSeconds(3);
        puede = true;
        yield return new WaitForSeconds(7);
        intro();
    }

    void Update()
    {
        if (Input.anyKeyDown && puede)
        {
            intro();
        }
    }
    public void intro()
    {
        anim.SetBool("intro", true);
    }

    public void getMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
