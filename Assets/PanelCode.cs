using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PanelCode : MonoBehaviour
{
    public MoveObject moveObject;
    [SerializeField] private Transform destino;
    [SerializeField] private Transform camPlayer;
    public GameObject gameObjectMoveCam;

    [SerializeField] private GameObject PLAYER;
    [SerializeField] private GameObject guide;
    [SerializeField] private GameObject guidePoint;

    [SerializeField] private MeshRenderer monitor;
    [SerializeField] private GameObject screen;

    [SerializeField] private Material monitorMaterial;
    [SerializeField] private PanelInside screenCode;
    [SerializeField] private PauseManager pauseManager;

    public void GetCam()
    {
        pauseManager.canPause = false;
        screenCode.fixBug = false;

        gameObjectMoveCam.SetActive(true);

        moveObject.SnapToTarget(camPlayer, destino, false);

        PLAYER.SetActive(false);
        guide.SetActive(false);
        guidePoint.SetActive(false);

        Material[] materials = monitor.materials; // Obtener copia del array de materiales
        materials[1] = monitorMaterial;            // Modificar el segundo material
        monitor.materials = materials;            // Asignar el array modificado de vuelta

        screenCode.canType = true;
        screen.SetActive(true);


    }
}
