using UnityEngine;

public class AnimAudioClip : MonoBehaviour
{
    public AudioSource asrc;

    public void PlayClip(AudioClip clip)
    {
        asrc.PlayOneShot(clip);
    }
}
