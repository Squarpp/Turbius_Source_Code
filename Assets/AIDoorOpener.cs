using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoorScript;

public class AIDoorOpener : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) // Cambiado de OnTriggerStay a OnTriggerEnter
    {
        if (other.CompareTag("door"))
        {
            Door door = other.GetComponent<Door>(); // Asegurar que sea el nombre correcto
            if (door != null)
            {
                door.RealOpenDoor();
            }
            else
            {
                Debug.LogError("No se encontró el componente DoorScript en el objeto con el tag 'door'");
            }
        }
    }

}
