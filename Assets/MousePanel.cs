using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePanel : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 cursorPosition;
    [SerializeField] private GameObject fps;


    // Límites en el espacio del juego (ajustalos a tu mapa o panel)
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -3f;
    public float maxY = 3f;

    private void Start()
    {
        cursorPosition = transform.position;
    }

    private void Update()
    {
        if (!fps.activeSelf)
        {
            Move();
        }
    }

    void Move()
    {
        float desplazaX = Input.GetAxis("Mouse X");
        float desplazaY = Input.GetAxis("Mouse Y");

        Vector3 mouseDesplazament = new Vector3(0f, desplazaY, desplazaX) * speed * Time.deltaTime;
        cursorPosition += mouseDesplazament;

        // Clampeamos para que no se pase de los límites
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, minY, maxY);
        cursorPosition.z = Mathf.Clamp(cursorPosition.z, minX, maxX);

        transform.position = cursorPosition;

        if (Input.GetMouseButtonDown(0)) // Clic izquierdo
        {
            Click();
        }
    }

    void Click()
    {
        // Asumimos que este objeto tiene un BoxCollider
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2f, transform.rotation);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("button"))
            {
                Debug.Log("¡Botón presionado: " + hit.name + "!");
                
                hit.GetComponent<ButtonPanel>().Click();
            }
        }
    }
}
