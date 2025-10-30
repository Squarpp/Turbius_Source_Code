using UnityEngine;
using VHS;

public class NewRunCam : MonoBehaviour
{
    public Animator AnimNew;
    public FirstPersonController FPS_m;
    [SerializeField] private MovementInputData movementInputData = null;
    // Update is called once per frame

    void Update()
    {
        if(movementInputData.IsRunning && FPS_m.isWalking && !FPS_m.IsCrouching)
        {
            AnimNew.SetBool("run", true);
        }
        else
        {
            AnimNew.SetBool("run", false);
        }
    }
}
