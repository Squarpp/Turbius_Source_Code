using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool enterOnceTime;
    [SerializeField] private GameObject invisibleWall;

    [SerializeField] private GameObject lvl1;
    [SerializeField] private GameObject lvl2;
    [SerializeField] private Transform lvl2SpawnPoint; // 🔹 Nuevo: punto donde se instanciará lvl2
    [SerializeField] private GameObject lucesAscensor;
    [SerializeField] private AudioSource asrcAscensor;
    [SerializeField] private AudioClip subiendoClip;
    [SerializeField] private AudioClip cerrandoClip;
    [SerializeField] private AudioClip abriendoClip;
    [SerializeField] private AudioSource asrcMusiquita;

    [SerializeField] private SMS sms;

    private void Start()
    {
        StartCoroutine(startmusiquita());
    }

    IEnumerator startmusiquita()
    {
        yield return new WaitForSeconds(0.1f);
        asrcMusiquita.Play();
    }

    public void Activate()
    {
        asrcAscensor.PlayOneShot(abriendoClip);
        _animator.SetBool("elevator", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !enterOnceTime)
        {
            StartCoroutine(introElevator());
            enterOnceTime = true;
        }
    }

    IEnumerator introElevator()
    {
        invisibleWall.SetActive(true);
        yield return new WaitForSeconds(1);
        _animator.SetBool("elevator", false);
        asrcAscensor.PlayOneShot(cerrandoClip);
        yield return new WaitForSeconds(2);

        if (sms != null) sms.backrooms_sms1_elevador();

        PlayerPrefs.SetInt("level", 3);
        lucesAscensor.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        asrcAscensor.PlayOneShot(subiendoClip);
        yield return StartCoroutine(LoadingLvl2part2Async()); // 🔹 Espera a que se destruya el lvl1
        lucesAscensor.SetActive(true);
        yield return new WaitForSeconds(2);

        if (sms != null) sms.backrooms_sms2_elevador();

        yield return new WaitForSeconds(3);

        if (sms != null) sms.backrooms_sms3_elevador();
        yield return new WaitForSeconds(3);
        lucesAscensor.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("InGame");

    }

    // 🔹 Versión asíncrona de carga con espera real
    IEnumerator LoadingLvl2part1Async()
    {
        GameObject newLvl2 = null;

        if (lvl2 != null)
        {
            // 🔹 Instanciar en el punto de spawn (posición + rotación)
            if (lvl2SpawnPoint != null)
                newLvl2 = Instantiate(lvl2, lvl2SpawnPoint.position, lvl2SpawnPoint.rotation);
            else
                newLvl2 = Instantiate(lvl2, Vector3.zero, Quaternion.identity);
        }

        // 🔹 Esperar hasta que el objeto exista y haya terminado su Awake/Start
        yield return new WaitUntil(() => newLvl2 != null);

        // 🔹 Pequeña espera extra por seguridad (por si carga assets pesados)
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator LoadingLvl2part2Async()
    {
        if (lvl1 != null)
        {
            Destroy(lvl1);
            yield return new WaitUntil(() => lvl1 == null); // 🔹 Espera real a que se destruya
        }
    }

    // Métodos sincrónicos por compatibilidad
    public void LoadingLvl2()
    {
        StartCoroutine(LoadingLvl2part1Async());
        StartCoroutine(LoadingLvl2part2Async());
    }
}
