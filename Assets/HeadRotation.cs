using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    public Transform player;  // Referencia al jugador
    public Transform defaulty;
    public float rotationSpeed_nochase = 5f; // Velocidad de rotación
    public float rotationSpeed_chase = 30f; // Velocidad de rotación
    public EnemyAI ai;

    private void Update()
    {
        if(!ai.scare)
        {
            if (ai.Chase)
            {
                FollowHead();
            }
            else
            {
                FollowDefault();
            }
        }
        else
        {
            
        }
    }

    void FollowHead()
    {
        if (player != null)
        {
            // Calcula la dirección desde el enemigo hacia el jugador
            Vector3 direction = player.position - transform.position;

            // Verifica que la dirección no sea cero para evitar errores
            if (direction != Vector3.zero)
            {
                // Calcula la rotación deseada
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Aplica una rotación suave usando Lerp
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed_chase);
            }
        }
    }
    void FollowDefault()
    {
        if (player != null)
        {
            // Calcula la dirección desde el enemigo hacia el jugador
            Vector3 direction = defaulty.position - transform.position;

            // Verifica que la dirección no sea cero para evitar errores
            if (direction != Vector3.zero)
            {
                // Calcula la rotación deseada
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Aplica una rotación suave usando Lerp
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed_nochase);
            }
        }
    }
}
