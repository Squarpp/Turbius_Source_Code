using UnityEngine;

public class MotionBlurData : MonoBehaviour
{
    public GameObject motionBlurGameObject;

    void Update()
    {
        int motionblur = PlayerPrefs.GetInt("MotionBlur", 1);
        // Si motionblur == 1, se activa. Si == 0, se desactiva.
        motionBlurGameObject.SetActive(motionblur == 1);
    }
}
