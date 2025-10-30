using UnityEngine;

public class PastoDetector : MonoBehaviour
{
    [SerializeField] private SoundEffects effects;

    private void OnCollisionStay(Collision collision)
    {
            // Guardamos el tag del objeto con el que colision�
            effects.getString = collision.gameObject.tag;
            Debug.Log("Terreno detectado: " + effects.getString);
    }
}