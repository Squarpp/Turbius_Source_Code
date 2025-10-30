using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public bool on;
    public Animator anim;
    public float power;
    public int phase;
    public Image img;
    public Sprite[] sprites;
    public float velocityPower;
    public Light flashLightt;


    // Update is called once per frame
    void Update()
    {
        anim.SetBool("on", on);
        if (Input.GetKeyDown(KeyCode.F) && Time.timeScale > 0)
        {
            on =! on;
        }

        if (on)
        {
            power = power-(Time.deltaTime * velocityPower);
        }
        else
        {
            power = power + ((Time.deltaTime / 2f) * velocityPower);
        }

        if (power <= 0)
        {
            flashLightt.intensity = 0;
        }

        if(power < 0)
        {
            power = 0;
        }

        if (power > 0 && power < 100)
        {
            flashLightt.intensity = 200;
            phase = 1;
        }

        if (power >= 1000 && power < 3000)
        {
            flashLightt.intensity = 500;
            phase = 1;
        }
        if (power >= 3000 && power < 6000)
        {
            flashLightt.intensity = 800;
            phase = 2;
        }
        if (power >= 6000 && power < 9000)
        {
            flashLightt.intensity = 900;
            phase = 3;
        }
        if (power >= 9000 && power < 11000)
        {
            flashLightt.intensity = 1000;
            phase = 4;
        }
        if (power > 11000)
        {
            power = 11000;
            phase = 4;
        }

        img.sprite = sprites[phase];

    }
}
