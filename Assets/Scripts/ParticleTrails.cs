using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(TrailRenderer))]
public class ParticleTrails : MonoBehaviour
{
    private ParticleSystem particles;
    private TrailRenderer trailRenderer;
    public float emitRate = 50f;
    public Gradient trailColor;
    public Gradient particlesColor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        trailRenderer = GetComponent<TrailRenderer>(); 

        trailRenderer.colorGradient = trailColor;
        var main = particles.main;
        main.startColor = particlesColor;
    }

    public void ActiveTrail()
    {
        // Enable the trail renderer
        trailRenderer.emitting = true;

        var em = particles.emission;
        em.rateOverDistance = emitRate;
        Debug.Log("Activated trail");
    }

    public void DisableTrail()
    {
        // Disable the trail renderer
        trailRenderer.emitting = false;

        var em = particles.emission;
        em.rateOverDistance = 0f;
        Debug.Log("Deactivated trail");
    }
}
