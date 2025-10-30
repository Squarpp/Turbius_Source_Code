using System.Collections;
using UnityEngine;

public class DialogSobrePokemon : MonoBehaviour
{
    [SerializeField] private AudioSource asrc;
    private bool alreadyPlayed = false; // controla si ya se ejecutó

    public bool isDucto;

    private void OnTriggerEnter(Collider other)
    {
        // Podés filtrar por tag si querés que solo funcione con el jugador
        if (!alreadyPlayed && other.CompareTag("Player"))
        {
            if (!isDucto)
            {
                int dialogPokemon = PlayerPrefs.GetInt("pokemon_dialog", 0);

                if (dialogPokemon == 0)
                {
                    StartCoroutine(playSound());
                    PlayerPrefs.SetInt("pokemon_dialog", 1);
                }
            }
            else
            {
                StartCoroutine(playSound());
            }

                alreadyPlayed = true;
        }
    }

    IEnumerator playSound()
    {
        yield return new WaitForSeconds(1);
        asrc.Play();
    }
}
