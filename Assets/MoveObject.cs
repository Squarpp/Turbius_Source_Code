using TMPro;
using UnityEngine;
using VHS;

public class MoveObject : MonoBehaviour
{
    public float speed = 2f;
    public GameObject fps;
    public TextMeshProUGUI txtGuide;
    public GameObject txtGuidePickObj;
    public InteractionInputData interactionInputData;
    public void MoveTo(Transform destination, bool ifGetFPS)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine(destination.position, destination.rotation, ifGetFPS));
    }

    public void SnapToTarget(Transform target, Transform dest, bool GETFPS)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;

        MoveTo(dest, GETFPS);
    }


    System.Collections.IEnumerator MoveCoroutine(Vector3 targetPos, Quaternion targetRot, bool ifFPS)
    {
        if(txtGuidePickObj != null) txtGuidePickObj.SetActive(false);
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            float smoothT = Mathf.SmoothStep(0f, 1f, t); // <- suavizado
            transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, smoothT);
            yield return null;
        }

        if (ifFPS)
        {
            fps.SetActive(true);

            interactionInputData.InteractedReleased = false;
            interactionInputData.InteractedReleased = true;

            interactionInputData.InteractedClicked = false;
            interactionInputData.InteractedClicked = true;

            txtGuide.text = "";
            gameObject.SetActive(false);
        }
        transform.position = targetPos;
        transform.rotation = targetRot;
    }
}
