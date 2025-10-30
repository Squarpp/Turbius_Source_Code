using System.Collections;
using TMPro;
using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]


    public class Door : MonoBehaviour {
        public bool open;
        public bool endingDoor;
        public bool locked;
        public bool activarTurbius; 
        private bool activarTurbiusUnaVez;
        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;
        public AudioSource asource;
        public AudioClip openDoor, closeDoor;
        public bool slenderScare;
        public SpiderJumpscare spiderJumpscare;
        public bool puertaMods;

        public TextMeshProUGUI subtitulos;
        public AudioSource asourceDialog;
        public AudioClip upsCreo;

        public GameObject TurbiusHelper;
        public GameObject TURBIUS;
        // Use this for initialization
        void Start () {
		asource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {


            if (!locked)
			{
                if (open)
                {
                    var target = Quaternion.Euler(0, DoorOpenAngle, 0);
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
                }
                else
                {
                    var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);

                }
			}
	}

        public void OpenDoor() {


            if (endingDoor)
            {
                CinematicaFinalSetup cinm = FindObjectOfType<CinematicaFinalSetup>();
                if (cinm != null)
                {
                    cinm.StartCinematica();
                }
                return;
            }

            if (spiderJumpscare != null)
            {
                if (slenderScare)
                {
                    spiderJumpscare.CAN = true;
                }
            }

            if (activarTurbius && !activarTurbiusUnaVez)
            {
                    if (TURBIUS != null) TURBIUS.SetActive(true); // Q MIEDO...
                    if (TurbiusHelper != null) TurbiusHelper.SetActive(true);
                    activarTurbiusUnaVez = true;
            }


            if (puertaMods)
            {
                int dialogDone = PlayerPrefs.GetInt("mods_dialog", 0);
                if(dialogDone == 0) 
                {
                    StartCoroutine(dialogMods());
                    PlayerPrefs.SetInt("mods_dialog", 1);
                }
            }

		open =!open;
		asource.clip = open?openDoor:closeDoor;
		asource.Play();
	}
        public void RealOpenDoor()
        {
            if (!open) {
                open = true;
                asource.clip = openDoor;
                asource.Play();
            }
        }

        IEnumerator dialogMods()
        {
            yield return new WaitForSeconds(1f);

            asourceDialog.clip = upsCreo;
            asourceDialog.Play();

            //ups

            subtitulos.text = "Ups";

            yield return new WaitForSeconds(0.52f);

            // creo
            subtitulos.text = "Creo";

            yield return new WaitForSeconds(0.8f);

            subtitulos.text = "Que se me ha olvidao de darles de comer.";

            yield return new WaitForSeconds(1.2f);

            subtitulos.text = "";
        }
    }
}