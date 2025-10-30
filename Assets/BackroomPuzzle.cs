using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BackroomPuzzle : MonoBehaviour
{
    public int itemsPuestos;
    public int itemLimit;

    public Material[] decalMaterials;
    public DecalProjector decalProjector;

    public GameObject AIFreddy;
    public GameObject AITurbius;
    public GameObject TurbiusSpawnColliders;
    public GameObject AIFreddySpawnColliders;
    public GameObject Luces;

    public SMS sms;

    [Header("Audio")]
    public AudioSource audioSource; // Arrastrar componente AudioSource
    public AudioSource audioSourceGeneral; // Arrastrar componente AudioSource

    public AudioClip addItemClip;   // Arrastrar clip en el inspector
    public AudioClip lightSound;
    public AudioClip apagon;
    public AudioClip keyDoneClip;

    public GameObject keyObj;

    public PickPlayer pickito;

    private bool freddy1Activado = false;
    private bool freddy2Activado = false;
    private bool turbius1Activado = false;
    private bool turbius2Activado = false;
    private bool turbius3Activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("canPickUp"))
        {
            foreach (var col in other.GetComponents<Collider>())
                col.enabled = false; // 🔒 desactiva todos los colliders

            functionAddProcess(other.gameObject);
        }
    }

    private void Start()
    {
       if(AIFreddy != null) AIFreddy.SetActive(false);
       if (AITurbius != null) AITurbius.SetActive(false);

        StartCoroutine(delayStartPorLasDudas());
    }

    IEnumerator delayStartPorLasDudas()
    {
        yield return new WaitForSeconds(1);
        while (pickito == null)
        {
            pickito = FindObjectOfType<PickPlayer>();
            yield return null; // Espera un frame
        }
    }


    private void Update()
    {
        if (pickito == null) return;

        // --- Freddy ---
        if (pickito.enemigoActivar == 1 && itemsPuestos >= 1 && !freddy1Activado)
        {
            AIFreddy.SetActive(true);
            freddy1Activado = true;
        }

        if (pickito.enemigoActivar == 1 && itemsPuestos >= 3 && !freddy2Activado)
        {
            AIFreddy.SetActive(true);
            AIFreddySpawnColliders.SetActive(true);
            freddy2Activado = true;
        }

        // --- Turbius ---
        if (pickito.enemigoActivar == 2 && itemsPuestos >= 3 && !turbius1Activado)
        {
            AITurbius.SetActive(true);
            turbius1Activado = true;
        }
        else if (pickito.enemigoActivar == 3 && itemsPuestos >= 2 && !turbius2Activado)
        {
            AITurbius.SetActive(true);
            turbius2Activado = true;
        }

        if (pickito.enemigoActivar > 2 && itemsPuestos >= 4 && !turbius3Activado)
        {
            AITurbius.SetActive(true);
            TurbiusSpawnColliders.SetActive(true);
            turbius3Activado = true;
        }
    }

    public void functionAddProcess(GameObject other)
    {
        sms = FindObjectOfType<SMS>();

        itemsPuestos++;
        Destroy(other.gameObject);

        // Cambiar material
        UpdateDecalMaterial();

        // Reproducir sonido
        if (audioSource != null && addItemClip != null)
        {
            audioSource.PlayOneShot(addItemClip);
        }

        // Buscar la linterna en la escena
        Flashlight flashlight = FindObjectOfType<Flashlight>();
        if (flashlight != null)
        {
            flashlight.power = 11000;
        }

        if(itemsPuestos == 1)
        {
            Luces.SetActive(false);
            audioSourceGeneral.PlayOneShot(apagon);
            audioSourceGeneral.PlayOneShot(lightSound);
            sms.startBackrooms2();
        }

        if (itemsPuestos > 4)
        {
            if (audioSourceGeneral != null) audioSourceGeneral.PlayOneShot(keyDoneClip);
           if (TurbiusSpawnColliders != null)  Destroy(TurbiusSpawnColliders);
            if (AIFreddySpawnColliders != null) Destroy(AIFreddySpawnColliders);
           if (AIFreddy != null) Destroy(AIFreddy);
           if(AITurbius != null) Destroy(AITurbius);
            if (keyObj != null) keyObj.SetActive(true);
            if (Luces != null) Luces.SetActive(true);
        }
    }


    void UpdateDecalMaterial()
    {
        int index = Mathf.Clamp(itemsPuestos, 0, decalMaterials.Length - 1);

        if (decalProjector != null && decalMaterials.Length > 0)
        {
            decalProjector.material = decalMaterials[index];
        }
    }
}
