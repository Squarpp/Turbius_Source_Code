using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VHS
{        
    public class InteractionUIPanel : MonoBehaviour
    {
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI tooltipText;

        public void SetTooltip(string tooltip)
        {
            tooltipText.SetText(tooltip);
        }

        public void UpdateProgressBar(float fillAmount)
        {
 
        }

        public void ResetUI()
        {
            tooltipText.SetText("");
        }
    }
}
