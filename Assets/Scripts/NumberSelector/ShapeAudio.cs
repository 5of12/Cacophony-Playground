using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShapeAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public float minCollisionVelocity = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        float velocity = collision.relativeVelocity.magnitude;
        if (collision.rigidbody != null && velocity > minCollisionVelocity && !audioSource.isPlaying)
        {
            audioSource.pitch = Random.Range(0.5f, 2f) + velocity / collision.rigidbody.mass;
            audioSource.volume = Mathf.Min(velocity - minCollisionVelocity, 1);
            audioSource.Play();
        }
    }
}
