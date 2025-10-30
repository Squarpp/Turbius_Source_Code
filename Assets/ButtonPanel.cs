using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Sprite spriteOn;
    [SerializeField] Sprite spriteOff;
    [SerializeField] private bool on;
    [SerializeField] private string type;
    [SerializeField] private PanelInside pInside;
    [SerializeField] private ClawCode cc;
    [SerializeField] private AudioSource asrc;
    [SerializeField] private AudioClip clip_click;
    public void Click()
    {
        switch (type)
        {
            case "onoff":
                OnOff();
            break;
            case "volver":
                Volver();
            break;
        }
    }
    public void OnOff()
    {
        on = !on;
        _sprite.sprite = on ? spriteOn : spriteOff;
        cc.activado = on;
        asrc.PlayOneShot(clip_click);
    }
    public void Volver()
    {
        asrc.PlayOneShot(clip_click);
        pInside.ReturnToFps();
    }
}
