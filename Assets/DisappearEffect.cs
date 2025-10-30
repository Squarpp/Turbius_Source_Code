using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearEffect : MonoBehaviour
{
    public GameObject target;
    public bool appear;
    private void OnTriggerEnter(Collider other)
    {
        if(target != null)
        {
            if (other.CompareTag("Player"))
            {
                target.SetActive(appear);
            }
        }
    }
}
