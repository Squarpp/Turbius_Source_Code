using UnityEngine;

public class freddyScareTrollActivator : MonoBehaviour
{
    public bool activate;
    public GameObject freddyLOL;
    public Transform transformAparittion;
    public AIFreddy aif;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && activate)
        {
            cagasteXD_opti_si_lees_esto_sos_GAY();
        }

    }
    void cagasteXD_opti_si_lees_esto_sos_GAY()
    {
        freddyLOL.SetActive(true);
        freddyLOL.transform.position = transformAparittion.transform.position;
        aif.chase = true;
    }
}
