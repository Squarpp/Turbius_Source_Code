using UnityEngine;

public class freddyScareTroll : MonoBehaviour
{
   public freddyScareTrollActivator fActivator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fActivator.activate = true;
        }
    }
}
