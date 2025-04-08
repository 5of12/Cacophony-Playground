using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(TrailRenderer))]
public class ParticleTrails : MonoBehaviour
{
    private ParticleSystem particles;
    public TrailRenderer trailRenderer;
    public float emitRate = 50f;
    public Gradient trailColor;
    public Gradient particlesColor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (particles == null)
        {
            particles = GetComponent<ParticleSystem>();
        }
        
        if (trailRenderer == null)
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }

        trailRenderer.colorGradient = trailColor;
        var main = particles.main;
        main.startColor = particlesColor;
    }

    public void ActivateTrail()
    {
        // Enable the trail renderer
        trailRenderer.emitting = true;

        var em = particles.emission;
        em.rateOverDistance = emitRate;
    }

    public void DisableTrail()
    {
        // Disable the trail renderer
        trailRenderer.emitting = false;

        var em = particles.emission;
        em.rateOverDistance = 0f;
    }
}
