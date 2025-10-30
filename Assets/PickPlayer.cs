using System.Collections;
using TMPro;
using UnityEngine;

public class PickPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    //if you copy from below this point, you are legally required to like the video
    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 5f; //how far the player can pickup the object from
    private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index
    public LayerMask pickUpLayer; // Asigna en el Inspector solo la capa de objetos recogibles

    public Material outlineMaterial;
    public AudioClip[] dropClip;

    public AudioSource asrc;
    public AudioClip mangelClip;

    public int enemigoActivar;
    public TextMeshProUGUI textGuide;

    public GameObject guide_E;

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    //MouseLookScript mouseLookScript;
    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""

        //mouseLookScript = player.GetComponent<MouseLookScript>();
    }
    void Update()
    {
        OutlineChecker();


        if(Time.timeScale > 0)
        {
            if (Input.GetKeyDown(KeyCode.E) && textGuide.text == "") //change E to whichever key you want to press to pick up
            {
                if (heldObj == null) //if currently not holding anything
                {
                    //perform raycast to check if player is looking at object within pickuprange
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange, pickUpLayer))
                    {
                        //make sure pickup tag is attached
                        if (hit.transform.gameObject.tag == "canPickUp")
                        {
                            //pass in object hit into the PickUpObject function
                            PickUpObject(hit.transform.gameObject);
                        }
                    }
                }
                else
                {
                    if (canDrop == true)
                    {
                        StopClipping(); //prevents object from clipping through walls
                        DropObject();
                    }
                }
            }
            if (heldObj != null) //if player is holding object
            {
                MoveObject(); //keep object position at holdPos
                RotateObject();
                if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
                {
                    StopClipping();
                    ThrowObject();
                }

            }
        }
    }


    private Renderer lastHighlightedRenderer; // Último objeto resaltado
    private Material originalMaterial; // Material original del objeto resaltado

    void OutlineChecker()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange, pickUpLayer))
        {
            if (hit.transform.CompareTag("canPickUp"))
            {
                Renderer objRenderer = hit.transform.GetComponent<Renderer>();

                if(guide_E != null) guide_E.SetActive(true);

                if (objRenderer != null)
                {
                    // Si el objeto es distinto al anterior, restauramos el anterior y guardamos el nuevo
                    if (lastHighlightedRenderer != objRenderer)
                    {
                        RemoveOutline(); // Eliminar el outline del anterior
                        lastHighlightedRenderer = objRenderer;
                        originalMaterial = objRenderer.materials[0]; // Guardar el material original
                    }

                    Material[] materials = objRenderer.materials;

                    // Si aún no tiene el material de outline, lo agregamos
                    if (materials.Length < 2 || materials[1] != outlineMaterial)
                    {
                        objRenderer.materials = new Material[] { originalMaterial, outlineMaterial };
                    }
                    return; // Salimos si encontramos un objeto
                }
            }
        }

        // Si el Raycast no detectó nada, eliminar el outline
        if (guide_E != null) guide_E.SetActive(false);
        RemoveOutline();
    }

    void RemoveOutline()
    {
        if (lastHighlightedRenderer != null)
        {
            lastHighlightedRenderer.materials = new Material[] { originalMaterial }; // Restaurar material original
            lastHighlightedRenderer = null; // Resetear
            originalMaterial = null;
        }
    }

    IEnumerator dialogPickVoice(string IDdialog)
    {
        int comprobar = PlayerPrefs.GetInt("rubiusDialog_mangel", 0);

        if (comprobar == 1) yield break; // mejor que return en una coroutine
        yield return new WaitForSeconds(1);
        switch (IDdialog)
        {
            case "mangel":
            asrc.PlayOneShot(mangelClip);
                PlayerPrefs.SetInt("rubiusDialog_mangel", 1);
            break;
        }
    }


    void PickUpObject(GameObject pickUpObj)
    {
        asrc.PlayOneShot(dropClip[Random.Range(0, dropClip.Length)]);
        if (pickUpObj.GetComponent<DialogPickUp>())
        {
            DialogPickUp DIAL = pickUpObj.GetComponent<DialogPickUp>(); // no dar bola

            if (DIAL.hasDialog)
            {
                StartCoroutine(dialogPickVoice(DIAL.IDdialog));
                DIAL.hasDialog = false;
            }

            if (DIAL.activaEnemigo > 0)
            {
                enemigoActivar = DIAL.activaEnemigo;
            }
        }


        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = 3; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        enemigoActivar = 0;
        //re-enable collision with player
        asrc.PlayOneShot(dropClip[Random.Range(0, dropClip.Length)]);
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 15; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }
    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))//hold R key to rotate, change this to whatever key you want
        {
            canDrop = false; //make sure throwing can't occur during rotating
            //disable player being able to look around
            //mouseLookScript.verticalSensitivity = 0f;
            //mouseLookScript.lateralSensitivity = 0f;

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis
            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            //re-enable player being able to look around
            //mouseLookScript.verticalSensitivity = originalvalue;
            //mouseLookScript.lateralSensitivity = originalvalue;
            canDrop = true;
        }
    }
    void ThrowObject()
    {
        enemigoActivar = 0;

        asrc.PlayOneShot(dropClip[Random.Range(0, dropClip.Length)]);
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 15;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;

        // 🔹 Empuja el objeto ligeramente hacia adelante antes del AddForce
        heldObj.transform.position = transform.position + transform.forward * 0.5f;

        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }

    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}