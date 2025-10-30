using UnityEngine;

public class Puzzle_1 : MonoBehaviour
{
    public int phase;
    public MeshRenderer render;
    public Material[] materialTo;
    public SMS sms;
    public Transform door;
    public Quaternion quat;
    private bool oneTime;
    [SerializeField] private GameObject key;
    [SerializeField] private Transform keySpawn;
    [SerializeField] private GameObject turbiusAI;
    [SerializeField] private GameObject slender;
    [SerializeField] private bool oneTime2; 
    [SerializeField] private AudioSource asrc;
    [SerializeField] private AudioClip clip;
    [SerializeField] public bool Checkpoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("canPickUp"))
        {
            DialogPickUp dp = other.GetComponent<DialogPickUp>();

            if (dp != null)
            {
                // acá verificás si el ID es "limon"
                if (dp.IDdialog == "limon")
                {
                    UpdatePhase();
                    Debug.Log("Se destruyó el limón supongo");
                    asrc.PlayOneShot(clip);
                    Destroy(other.gameObject);
                }
            }
        }
    }

    private void Update()
    {
        if (!oneTime)
        {
            if (phase > 2 && !Checkpoint)
            {
                Instantiate(key, keySpawn.position, keySpawn.rotation);
                door.rotation = quat;
                sms.BienHechoFunc();
                oneTime = true;
            }
        }
        if (!oneTime2)
        {
            if (phase > 1)
            {
                int slenderCan = PlayerPrefs.GetInt("slenderCan", 0);
                if(slenderCan == 0)
                {
                    slender.SetActive(true);
                }
                oneTime2 = true;
            }
        }
    }

    public void UpdatePhase()
    {
        if (phase < materialTo.Length) // Asegurar que no se salga del rango del array
        {
            phase++;
            Material[] materials = render.materials; // Obtener todos los materiales
            if (materials.Length > 1) // Verificar que el objeto tenga más de un material
            {
                materials[1] = materialTo[phase];
                render.materials = materials; // Asignar el array modificado de nuevo
            }
        }
    }
}

