using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class LowRes : MonoBehaviour
{
    void Start()
    {
        DynamicResolutionHandler.SetDynamicResScaler(SetDynamicResolutionScale, DynamicResScalePolicyType.ReturnsPercentage);
    }

    float SetDynamicResolutionScale()
    {
        return Mathf.Clamp01(640f / (float)Screen.width);
    }
}
