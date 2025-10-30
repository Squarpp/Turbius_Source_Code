using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonPokemon : MonoBehaviour
{
    public GameBoy gb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Pushable"))
        {
            gb.GetBack();
        }
    }
}
