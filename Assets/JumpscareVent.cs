using UnityEngine;

public class JumpscareVent : MonoBehaviour
{
    [SerializeField] private GameObject aiObj;
    [SerializeField] private EnemyAI ai;
    private void OnTriggerEnter(Collider other) // Cambiado de OnTriggerStay a OnTriggerEnter
    {
        if (other.CompareTag("Player"))
        {
            aiObj.SetActive(true);
            ai.scare = true;
        }
    }

}
