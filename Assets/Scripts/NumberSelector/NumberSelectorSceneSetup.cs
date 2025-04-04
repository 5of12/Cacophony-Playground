using UnityEngine;

public class NumberSelectorSceneSetup : MonoBehaviour
{
    [Header("Fog Settings")]
    public Color fogColor = new Color(0.5f, 0.5f, 0.5f);
    public FogMode fogMode = FogMode.Linear;
    public float fogDensity = 0.05f;
    public float fogStartDistance = 1f;
    public float fogEndDistance = 3f;
    public float ambientIntensity = 0.4f;

    [Header("Physics Settings")]
    public float gravity = -2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity = new Vector3(0, gravity, 0);

        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.ambientIntensity = ambientIntensity;
        RenderSettings.fogStartDistance = fogStartDistance;
        RenderSettings.fogEndDistance = fogEndDistance;
        RenderSettings.fogMode = fogMode;
    }
}
