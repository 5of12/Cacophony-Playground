using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(TrailRenderer))]
public class ParticleTrails : MonoBehaviour
{
    private ParticleSystem particles;
    public TrailRenderer trailRenderer;

    [SerializeField]
    private float _emitRate = 50f;
    public float EmitRate {
        get { return _emitRate; }
        set { _emitRate = value; SetEmitRate(_emitRate); }
    }
    public Gradient trailColor;
    public Gradient particlesColor;
    public Light originLight;

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

        SetEmitRate(_emitRate);
        if (originLight != null)
        {
            originLight.enabled = true;
        }
    }

    public void DisableTrail()
    {
        // Disable the trail renderer
        trailRenderer.emitting = false;

        EmitRate = 0;
        if (originLight != null)
        {
            originLight.enabled = false;
        }
    }

    public void SetPosition(Vector3 position, bool smooth = false)
    {
        if (smooth)
        {
            SmoothMove(position);
        }
        else
        {
            transform.position = position;
        }
    }
    private void SmoothMove(Vector3 position)
    {
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 10f);
    }

    public void SetColor(Gradient color)
    {
        trailRenderer.colorGradient = color;
        var main = particles.main;
        main.startColor = color;

        if (originLight != null)
        {
            originLight.color = color.colorKeys[0].color;
        }
    }

    public void SetEmitRate(float rate)
    {
        var em = particles.emission;
        em.rateOverDistance = rate;
    }
}
