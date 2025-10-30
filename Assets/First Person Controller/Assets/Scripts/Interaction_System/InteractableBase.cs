using UnityEngine;
using NaughtyAttributes;

namespace VHS
{
    public class InteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Interactable Settings")]
        [SerializeField] private bool holdInteract = true;
        [ShowIf("holdInteract")][SerializeField] private float holdDuration = 1f;
        [SerializeField] private bool multipleUse = false;
        [SerializeField] public bool isInteractable = true;
        [SerializeField] public string tooltipMessage = "interact";
        [SerializeField] public string type = "";
        [SerializeField] public string infoItem = "";
        [SerializeField] public string infoItemTitle = "";
        [SerializeField] public int idInfoObj;
        [SerializeField] public string keyName = "";
        [SerializeField] public string lockedPass = "";
        [SerializeField] public Rigidbody trapBody;
        [SerializeField] public BoxCollider trapCollider;
        [SerializeField] public LayerMask layerPlayer;

        [Header("Components")]
        [SerializeField] private DoorScript.Door door;
        [SerializeField] private DrawerScript.Drawer drawer;
        [SerializeField] private Animator anim;
        public InteractionController fpsController;
        public GameBoy gb;
        public PanelCode panelCode;
        public Lever lever;
        public Elevator elevator;
        public ClawCode clawCode;
        public PauseManager pauseManager;

        [SerializeField] private AudioSource audioSource;
        public float HoldDuration => holdDuration;
        public bool HoldInteract => holdInteract;
        public bool MultipleUse => multipleUse;
        public bool IsInteractable => isInteractable;
        public string TooltipMessage => tooltipMessage;

        [SerializeField] private Animator brazoPick;



        private void Start()
        {
            fpsController = FindObjectOfType<InteractionController>();

   
        }

        public virtual void OnInteract()
        {
            if (isInteractable && tooltipMessage == "panel")
            {
                type = "''E'' para entrar";
            }

            if (fpsController == null)
                fpsController = FindObjectOfType<InteractionController>();

            switch (type)
            {
                case "door":
                    TryOpenLockable(door);
                    break;

                case "drawer":
                    TryOpenLockable(drawer);
                    break;

                case "key":
                    TryPickKey();
                    break;

                case "ItemInfo":
                    pauseManager.fixPause = 0;
                    fpsController?.GetInfo(infoItem, infoItemTitle, idInfoObj);
                    break;

                case "gb":
                    gb?.SetScreenGB();
                    break;

                case "panel":
                    panelCode?.GetCam();
                    break;

                case "lever":
                    lever?.Activate();
                    break;

                case "elevator":
                    elevator.Activate();
                    tooltipMessage = "";
                    isInteractable = false;
                    break;

                case "claw":
                    clawCode.SetClawCam();
                    break;

                case "trap":
                    trapBody.freezeRotation = false;
                    trapBody.isKinematic = false;
                    trapCollider.excludeLayers = layerPlayer;

                    PushDomino(Vector3.back);

                    isInteractable = false;
                    tooltipMessage = "";

                    break;
            }

            Debug.Log("INTERACTED: " + gameObject.name);
        }

        public void PushDomino(Vector3 direction, float force = 10f)
        {
            brazoPick.SetBool("destornillador", false);
            audioSource.Play();
            // Empuje hacia la dirección indicada
            trapBody.AddForce(direction.normalized * -force, ForceMode.Impulse);

            // Además, torque para que rote como ficha
            Vector3 torque = Vector3.Cross(Vector3.up, direction) * force;
            trapBody.AddTorque(torque, ForceMode.Impulse);
        }


        private void TryOpenLockable(object lockable)
        {
            if (lockable is DoorScript.Door d)
            {
                if (!d.locked || HasCorrectKey())
                {
                    d.locked = false;
                    d.OpenDoor();
                }
                else fpsController?.lockedAnim();
            }
            else if (lockable is DrawerScript.Drawer dr)
            {
                if (!dr.locked || HasCorrectKey())
                {
                    dr.locked = false;
                    dr.OpenDrawer();
                }
                else fpsController?.lockedAnim();
            }
        }

        private bool HasCorrectKey()
        {
            if (fpsController.setKey == lockedPass)
            {
                fpsController.setKey = "";
                return true;
            }
            return false;
        }

        private void TryPickKey()
        {
            if (fpsController == null) return;

            if (string.IsNullOrEmpty(fpsController.setKey))
            {
                fpsController.setKey = keyName;
                fpsController.PlayAnimKey();
                Destroy(gameObject);
            }
            else
            {
                fpsController.PlayErrorKey();
            }
        }
    }
}
