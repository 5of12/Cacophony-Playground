using UnityEngine;

public class NumberSelectorSceneSetup : MonoBehaviour
{
    public Color fogColor = new Color(0.5f, 0.5f, 0.5f);
    public float fogDensity = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity = new Vector3(0, -2f, 0);

        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.ambientIntensity = 0.4f;
    }
}
