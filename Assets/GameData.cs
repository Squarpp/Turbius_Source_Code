using DoorScript;
using DrawerScript;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using VHS;
using static UnityEngine.Rendering.DebugUI;

public class GameData : MonoBehaviour
{
    public int checkpoint; //es un static que se guarda como progreso

    public GameObject introTimeLine;
    public GameObject Player;
    public GameObject SillaGamer;
    public GameObject Mapa;
    public GameObject UI;

    public TextMeshProUGUI subtitulos;

    public PlayableDirector introTimelinePlay;

    public SMS sms;

    public GameObject Flashlight;
    public SphereCollider ElevatorCollider;

    public AudioSource audioSource;
    public AudioClip[] aclipsRubius;
    public AudioClip[] aclipsMangel;

    public bool debugCheckpoint;
    [SerializeField] private bool debugTeleporter;
    [SerializeField] private GameObject Backrooms;
    [SerializeField] private GameObject HabitacionRubius;
    [SerializeField] private GameObject fadeout;
    [SerializeField] private Transform bb1Checkpoint;
    [SerializeField] private Transform bb2Checkpoint;
    [SerializeField] private Transform bb3Checkpoint;
    [SerializeField] private Drawer drawerPuertaPatio;
    [SerializeField] private Door puerta_pokemon;
    [SerializeField] private InteractableBase pokemonJuego;
    [SerializeField] private Puzzle_1 puzzleLimones;
    [SerializeField] private Lever lever;
    [SerializeField] private GameObject[] limonesObj;
    [SerializeField] private GameObject backroomsPrefab;
    [SerializeField] private GameObject fadeOut;

    [SerializeField] private GameObject configuracionObj;

    [SerializeField] private AudioMixer audioMixer;


    private void Awake()
    {
        if (fadeOut != null) fadeOut.SetActive(true);
        float vol = PlayerPrefs.GetFloat("Volumen", 0.75f); // fixear ligero bug
        audioMixer.SetFloat("MasterVolume", vol > 0 ? Mathf.Log10(vol) * 20 : -80f);
        if (!debugCheckpoint)
        {
            checkpoint = PlayerPrefs.GetInt("level", 0);
        }

        Flashlight.SetActive(false);
        UI.SetActive(false);
        Player.SetActive(false);
        SillaGamer.SetActive(false);
        Mapa.SetActive(false);
        introTimeLine.SetActive(false);
    }


    void Start()
    {
        Time.timeScale = 1.0f;
        introTimelinePlay.stopped += OnTimelineStopped;

        StartCoroutine(loading());
    }

    IEnumerator loading()
    {
        yield return new WaitForEndOfFrame();
        // Instanciamos el objeto y lo guardamos
        GameObject tempConfig = Instantiate(configuracionObj);

            // Intentamos desactivar el background si existe
            Transform bg = tempConfig.transform.Find("BackgroundVisualConfig");
            if (bg != null)
                bg.gameObject.SetActive(false);

            // Esperamos un poquito para que se inicialice la lógica
            yield return new WaitForEndOfFrame();

            // Luego lo destruimos
            Destroy(tempConfig);

        yield return new WaitForSeconds(0.25f);

        switch (checkpoint)
        {
            case 0: //intro
                setIntro();
                break;

            case 1: //setup

                endIntro();
                sms.startFirstDialogue();
                Flashlight.SetActive(true);

                break;

            case 2: //apareces en el ascensor y en los backrooms

                checkpoint2();
                Flashlight.SetActive(true);
                break;


            case 3: //apareces en el ascensor y en los backrooms

                checkpoint3();
                Flashlight.SetActive(true);

                yield return new WaitForEndOfFrame(); // que ande por favor la concha de la lora
                yield return new WaitForSeconds(0.1f);

                checkpoint3fixBug();
                break;
        }
    }



    void OnTimelineStopped(PlayableDirector obj)
    {
            endIntro();
    }
    void setIntro()
    {
        Flashlight.SetActive(false);
        UI.SetActive(false);
        Player.SetActive(false);
        SillaGamer.SetActive(false);
        Mapa.SetActive(false);
        introTimeLine.SetActive(true);
    }

    void endIntro()
    {
        UI.SetActive(true);
        Player.SetActive(true);
        SillaGamer.SetActive(true);
        Mapa.SetActive(true);
        Destroy(introTimeLine);

        if (!debugTeleporter)
        {
            Player.transform.position = bb1Checkpoint.transform.position;
            Player.transform.rotation = bb1Checkpoint.transform.rotation;
        }

        if (checkpoint == 0)
        {
            IntroCallMangel();
            checkpoint = 1;
            PlayerPrefs.SetInt("level", 1);
        }
    }
    void checkpoint2()
    {
        UI.SetActive(true);
        Player.SetActive(true);
        SillaGamer.SetActive(true);
        Mapa.SetActive(true);
        fadeout.SetActive(true);

        Destroy(introTimeLine);

        drawerPuertaPatio.locked = false;
        puerta_pokemon.locked = false;
        pokemonJuego.isInteractable = false;
        pokemonJuego.tooltipMessage = "";
        puzzleLimones.Checkpoint = true;
        puzzleLimones.phase = 2;
        puzzleLimones.UpdatePhase();

        for (int i = 0; i < limonesObj.Length; i++)
        {
            Destroy(limonesObj[i]);
        }

        lever.funcionActivate = true;
        lever.Activate();

        if (Backrooms != null)
        {
            Destroy(Backrooms);
        }

        CharacterController cc = Player.GetComponent<CharacterController>();

        cc.enabled = false;
        Player.transform.position = bb2Checkpoint.transform.position;
        Player.transform.rotation = bb2Checkpoint.transform.rotation;



        cc.enabled = true;
        Destroy(introTimeLine);
    }

    void checkpoint3()
    {
        // instanciamos usando el prefab, no el objeto destruido
        if (backroomsPrefab != null)
            Instantiate(backroomsPrefab);

        if (Mapa != null) Destroy(Mapa);
        if (HabitacionRubius != null) Destroy(HabitacionRubius);
        if (introTimeLine != null) Destroy(introTimeLine);
    }

    void checkpoint3fixBug()
    {
        CharacterController cc = Player.GetComponent<CharacterController>();

        UI.SetActive(true);
        Player.SetActive(true);
        fadeout.SetActive(true);
        cc.enabled = false;
        Player.transform.position = bb3Checkpoint.transform.position;
        Player.transform.rotation = bb3Checkpoint.transform.rotation;
        cc.enabled = true;
        ElevatorCollider.enabled = false;

        sms.startFirstDialogueBackrooms();
    }

        void IntroCallMangel()
    {
        StartCoroutine(llamarMangel());
    }

    IEnumerator llamarMangel()
    {
        subtitulos.text = "MANGEL: ¿Ruben estas ahí?";

        audioSource.PlayOneShot(aclipsMangel[0]);

        yield return new WaitForSeconds(1f);

        subtitulos.text = "";

        yield return new WaitForSeconds(0.5f);

        subtitulos.text = "RUBÉN: Hey ¿que pasa?";

        audioSource.PlayOneShot(aclipsRubius[0]);

        yield return new WaitForSeconds(1.5f);

        subtitulos.text = "MANGEL: Que me ha raptao el mono este";

        audioSource.PlayOneShot(aclipsMangel[1]);

        yield return new WaitForSeconds(2f);

        subtitulos.text = "RUBÉN: ¿Espera que?";

        audioSource.PlayOneShot(aclipsRubius[1]);

        yield return new WaitForSeconds(2f);

        subtitulos.text = "RUBÉN: ¿De que coño me estás hablando tío?";

        audioSource.PlayOneShot(aclipsRubius[2]);

        yield return new WaitForSeconds(2f);

        subtitulos.text = "MANGEL: ¡QUE TÍO TE LO ESTOY DICIENDO!";

        audioSource.PlayOneShot(aclipsMangel[2]);

        yield return new WaitForSeconds(3f);

        subtitulos.text = "MANGEL: Me ha enjaulado ya sabes quien...";

        audioSource.PlayOneShot(aclipsMangel[3]);

        yield return new WaitForSeconds(3);

        subtitulos.text = "RUBÉN: E- Oh no...";

        audioSource.PlayOneShot(aclipsRubius[3]);

        yield return new WaitForSeconds(1f);

        subtitulos.text = "";

        yield return new WaitForSeconds(0.8f);

        subtitulos.text = "MANGEL: Así es...";

        audioSource.PlayOneShot(aclipsMangel[4]);

        yield return new WaitForSeconds(1.2f);

        subtitulos.text = "RUBÉN: Vale.";

        audioSource.PlayOneShot(aclipsRubius[4]);

        yield return new WaitForSeconds(0.7f);

        subtitulos.text = "";

        yield return new WaitForSeconds(0.2f);

        subtitulos.text = "*VOZ INTELIGIBLE*";

        audioSource.PlayOneShot(aclipsRubius[5]);

        yield return new WaitForSeconds(1.5f);

        subtitulos.text = ""; 

        yield return new WaitForSeconds(1);

        sms.startFirstDialogue();
        Flashlight.SetActive(true);

    }
}
