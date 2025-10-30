using UnityEngine;

public class Invocar_Turbius_AI_vent : MonoBehaviour
{
    [SerializeField] private GameObject aiObj;
    [SerializeField] private GameObject aiObjVent;
    [SerializeField] private EnemyAI ai;
    private void OnTriggerEnter(Collider other) // Cambiado de OnTriggerStay a OnTriggerEnter
    {
        if (other.CompareTag("Player"))
        {
            aiObj.SetActive(true);
            aiObjVent.SetActive(true);
            ai.Chase = true;
        }
    }
}
