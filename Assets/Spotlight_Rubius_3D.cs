using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight_Rubius_3D : MonoBehaviour
{
    public GameObject spot;
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip clickOn;
    [SerializeField] private AudioClip clickOff;

    public void spotlightOn()
    {
        src.PlayOneShot(clickOn);
        spot.SetActive(true);
    }
    public void spotlightOff()
    {
        src.PlayOneShot(clickOff);
        spot.SetActive(false);
    }
}
