using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouGot : MonoBehaviour
{
    public GameObject canvas;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Out();
        }
    }
    public void Out()
    {
        Animator animator;
        animator = GetComponent<Animator>();
        animator.SetBool("out", true);
    }
    public void Quit()
    {
        canvas.SetActive(false);
    }
}
