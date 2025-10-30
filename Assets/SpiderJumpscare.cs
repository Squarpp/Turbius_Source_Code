using DoorScript;
using System.Collections;
using UnityEngine;
using VHS;
public class SpiderJumpscare : MonoBehaviour
{
    public Animator anim;
    public Transform playerCamera; // Asigna aquí la cámara principal
    public float viewThreshold = 0.9f; // Ajusta según el campo de visión
    public bool lookplayer;
    public FirstPersonController fps;
    public GameObject img;
    public bool CAN = true;
    [SerializeField] private AudioSource ac;
    [SerializeField] private AudioClip aclip;
    [SerializeField] private bool isSlenderman;
    private bool onetime;
    public Door puertaSlenderman;
    public bool turbiusCorredor;
    public bool spider;
    public bool shrek;

    private void Start()
    {
            int isJumpscareOnCorredor = PlayerPrefs.GetInt("turbiusMiniJumpscare", 0);
            if (isJumpscareOnCorredor == 1 && turbiusCorredor)
            {
               Destroy(gameObject);
            }

            int isJumpscareOnSpider = PlayerPrefs.GetInt("spiderMiniJumpscare", 0);
        if (isJumpscareOnSpider == 1 && spider)
        {
            Destroy(gameObject);
        }

        int isJumpscareShrek = PlayerPrefs.GetInt("shrekMiniJumpscare", 0);
        if (isJumpscareShrek == 1 && shrek)
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (lookplayer && CAN)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!isSlenderman)
                {
                    fps.temporalScare();
                }
                else
                {
                    if(puertaSlenderman != null && puertaSlenderman.open)
                    {
                        if (!onetime)
                        {
                            ac.PlayOneShot(aclip);
                            PlayerPrefs.SetInt("slenderCan", 1);
                            onetime = true;
                        }
                    }
                }
                    anim.Play("jumpscare");
            }
        }
    }
    void Update()
    {

        Vector3 dirToObject = (transform.position - playerCamera.position).normalized;
        float dotProduct = Vector3.Dot(playerCamera.forward, dirToObject);

        if (dotProduct > viewThreshold) // Si está dentro del campo de visión
        {
            lookplayer = true;
        }
        else
        {
            lookplayer = false;
        }
    }
    public void Slenderman()
    {
        StartCoroutine(slenderCount());
    }
    IEnumerator slenderCount()
    {
        img.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        img.SetActive(false);
        Destroy(gameObject);
    }
}
