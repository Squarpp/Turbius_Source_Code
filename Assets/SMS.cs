using System.Collections;
using TMPro;
using UnityEngine;

public class SMS : MonoBehaviour
{
    public GameObject SMSPrefab;
    public GameObject SMSMenuPrefab;
    public Transform T;
    public Transform TMenu;


    public void startFirstDialogue()
    {
        StartCoroutine(cocinaa());
    }
    public void startFirstDialogueBackrooms()
    {
        StartCoroutine(Backrooms());
    }
    public void startBackrooms2()
    {
        StartCoroutine(Backrooms2());
    }
    public void truquito()
    {
        StartSMS("Squarp: <color=red>ese truco ya me lo sé.</color>");
    }

    public void publicTrampilla()
    {
        StartCoroutine(trampillaFunc());
    }


    public void cagastemirey()
    {
        StartSMS("Squarp: cagaste mi rey");
    }


    IEnumerator cocinaa()
    {
        yield return new WaitForSeconds(2);
        StartSMS("SMS: Holaaa! he vuelto :)");
        yield return new WaitForSeconds(2);

        StartSMS("SMS: he bloqueado el pasillo del segundo piso... deberais hacer lo que te diga para darte las llaves :)");

        yield return new WaitForSeconds(2);

        yield return new WaitForSeconds(2);
        SMSCocina();
    }


    IEnumerator trampillaFunc()
    {
        yield return new WaitForSeconds(2.5f);
        trampilla();
    }




    void SMSCocina()
    {
        StartSMS("SMS: Consigue 3 <color=yellow>limones</color> y entregamelos al contenedor. Quiero comer.");
    }

    void trampilla()
    {
        StartSMS("SMS: Ve al baño y abre la trampilla.");
    }

    void SMSIntro()
    {
        StartSMS("SMS: Ve al patio, arregla los cables y activad el interruptor.");
    }

    void SMSSetup()
    {
        StartSMS("SMS: Busca el código, escríbelo en la PC DE LA NASA y consigue el <color=#3e66f5>DESTORNILLADOR</color>.");
    }
    void BienHecho()
    {
        StartSMS("SMS: ok... muy bien hecho...");
    }

    public void mangel_hospital(int type)
    {
        switch (type)
        {
            case 1:
                StartSMS("Mangel: Avisame cuando despiertes");
                break;

            case 2:
                StartSMS("Mangel: Este gaspi to loco te ha disparao...");
                break;

            case 3:
                StartSMS("Mangel: Y alguien te trajo al hospital.");
                break;

            case 4:
                StartSMS("Te sientes mejor??");
                break;
        }
    }

    /// <summary>
    /// BACKROOMS
    /// DIÁLOGOS
    /// </summary>
    public void backrooms_sms1_elevador()
    {
        StartSMS("<color=yellow>Has agendado el contacto como <b>Turbius</b>.</color>");
    }
    public void backrooms_sms2_elevador()
    {
        StartSMS("<b>Turbius:</b> Venga tío, tu sabes en que lio te has metido.");
    }
    public void backrooms_sms3_elevador()
    {
        StartSMS("<b>Turbius:</b> Tal vez te guste mi casa. Es un tanto... Grande je.");
    }



    void backrooms_sms2()
    {
        StartSMS("<b>Turbius:</b> Consigue 5 objetos y entrégalos al agujero cerca de mi habitación.");
    }

    void backrooms_sms3()
    {
        StartSMS("<b>Turbius:</b> Quiero juguetes para divertirme un rato :)");
    }

    void backrooms_sms4()
    {
        StartSMS("<b>Turbius:</b> He traido un amigo a jugar :)");
    }

    void backrooms_sms5()
    {
        StartSMS("<b>Turbius:</b> Vendrá dentro de poco, tal vez te suene.");
    }
    void backrooms_sms6()
    {
        StartSMS("<b>Turbius:</b> Ten cuidado con el, no le gusta mucho la gente que digamos.");
    }



    /// <summary>
    /// ESOS SON TODOS
    /// </summary>

    void Jejeje()
    {
        StartSMS("SMS: Jejejej no debiste hacer eso.");
    }
    void PeroEstaBien()
    {
        StartSMS("Pero está bien aqui tienes tu llave :)");
    }

    public void BienHechoFunc()
    {
        StartCoroutine(BienHechoTemp());
    }

    public void JejejeFunc()
    {
        StartCoroutine(JejejeTemp());
    }

    IEnumerator BienHechoTemp()
    {
        yield return new WaitForSeconds(2);
        BienHecho();
        yield return new WaitForSeconds(2);
        SMSIntro();
    }
    IEnumerator Backrooms()
    {
        yield return new WaitForSeconds(2);
        backrooms_sms2();
        yield return new WaitForSeconds(4);
        backrooms_sms3();
    }

    IEnumerator Backrooms2()
    {
        yield return new WaitForSeconds(2);
        backrooms_sms4();
        yield return new WaitForSeconds(4);
        backrooms_sms5();
        yield return new WaitForSeconds(4);
        backrooms_sms6();
    }


    IEnumerator JejejeTemp()
    {
        yield return new WaitForSeconds(3);

        Jejeje();

        yield return new WaitForSeconds(3);

        PeroEstaBien();

        yield return new WaitForSeconds(4);

        SMSSetup();
    }

    public void StartSMS(string txt)
    {
        // Instanciar el SMS en la pantalla principal
        GameObject pref = Instantiate(SMSPrefab, T);
        TextMeshProUGUI textComponent = pref.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = txt;
        }
        else
        {
            Debug.LogWarning("No se encontró el componente TextMeshProUGUI en SMSPrefab.");
        }

        // Instanciar el SMS en el menú
        GameObject prefMenu = Instantiate(SMSMenuPrefab, TMenu);
        TextMeshProUGUI textComponentMenu = prefMenu.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponentMenu != null)
        {
            textComponentMenu.text = txt;
        }
        else
        {
            Debug.LogWarning("No se encontró el componente TextMeshProUGUI en SMSMenuPrefab.");
        }
    }
}
