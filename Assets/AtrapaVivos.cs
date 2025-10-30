using System.Collections.Generic;
using UnityEngine;

public class AtrapaVivos : MonoBehaviour
{
    [Header("Configuración de detección")]
    public float detectionRadius = 5f;
    public LayerMask detectionMask;

    private bool multipleDetectados = false;
    private bool timerActivo = false;
    private float timer = 0f;

    public GameObject freddyObj;
    public AIFreddy aifreddy;
    private bool evento3Activado = false;
    private bool evento6Activado = false;

    public SMS sms;


    void Update()
    {
        if(sms != null) sms = FindObjectOfType<SMS>();

        DetectarObjetos();

        if (timerActivo)
        {
            timer += Time.deltaTime;

            if (!evento3Activado && timer >= 3f)
            {
                sms.truquito();
                evento3Activado = true;
            }

            if (!evento6Activado && timer >= 6f)
            {
                sms.cagastemirey();
                freddyObj.SetActive(true);
                aifreddy.chase = true;
                evento6Activado = true;
            }
        }
    }

    void DetectarObjetos()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);
        List<DialogPickUp> objetosValidos = new List<DialogPickUp>();

        foreach (var hit in hits)
        {
            if (hit.CompareTag("canPickUp"))
            {
                DialogPickUp pick = hit.GetComponent<DialogPickUp>();
                if (pick != null)
                    objetosValidos.Add(pick);
            }
        }

        multipleDetectados = false;

        if (objetosValidos.Count > 2)
        {
            foreach (var obj in objetosValidos)
            {
                if (obj.activaEnemigo == 1)
                {
                    multipleDetectados = true;

                    if (!timerActivo)
                    {
                        timerActivo = true;
                        timer = 0f; // empieza desde cero solo una vez
                        Debug.Log("🚨 Detección múltiple: ¡timer iniciado!");
                    }

                    break;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = multipleDetectados ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
