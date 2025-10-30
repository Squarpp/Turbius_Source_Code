using UnityEngine;
using VHS;

public class BugFixerTurbiusPanelFurby : MonoBehaviour
{
    public EnemyAI enemyAI;
    public InteractableBase iBase;

    void Update()
    {
        if(enemyAI != null && enemyAI.Chase)
        {
            iBase.type = "";
        }
        else
        {
            iBase.type = "panel";
        }
    }
}
