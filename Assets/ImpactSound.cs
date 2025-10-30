using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ImpactSound : MonoBehaviour
{
    public AudioSource audioSource; // Arrastrar un AudioSource
    public AudioClip[] impactClip;    // Clip de golpe
    public float minImpactForce = 2f; // Fuerza mínima para que suene

    private void OnCollisionEnter(Collision collision)
    {
        // Detectar fuerza del impacto
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce >= minImpactForce)
        {
            if (audioSource != null && impactClip != null)
            {
                audioSource.PlayOneShot(impactClip[Random.Range(0, impactClip.Length)]);
            }
        }
    }
}
