using UnityEngine;

public class MaterialSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator action;
    [SerializeField] private bool isChapa;
    [SerializeField] private bool noTirar;


    [Header("Sonidos por tipo de superficie")]
    public AudioClip sueloPasto;

    private void Start()
    {
        int _scareChapa = PlayerPrefs.GetInt("scareChapa", 0);

        if (_scareChapa == 1) noTirar = true;

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    public void Sound()
    {
        audioSource.PlayOneShot(sueloPasto);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isChapa) PlayerPrefs.SetInt("scareChapa", 1);

            if(!noTirar) action.SetBool("activ", true);
        }
    }
}
