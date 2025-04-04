using UnityEngine;

namespace Cacophony
{
    [RequireComponent(typeof(AudioSource))]
    public class GestureDroneAudio : IGestureConsumer
    {
        public AudioSource audioSource;
        public float debounceTimer = 0.1f;
        public Vector2 pitchMinMax = new Vector2(0.5f, 1.5f);
        public Vector2 volumeMinMax = new Vector2(0f, 1f);
        public float lerpSpeed = 5f;
        private float targetPitch = 1f;
        private float targetVolume = 1f;

        private bool started = false;
        private float endTime = 0f;

        protected override void Awake()
        {
            base.Awake();
            if (audioSource == null){
                audioSource = GetComponent<AudioSource>();
            }
            targetPitch = pitchMinMax.x;
            targetVolume = volumeMinMax.x;
            audioSource.PlayDelayed(Random.Range(0f, 2f));
        }

        protected override void HandleStart(ActionEventArgs eventData)
        {
            if (!started && Time.time - endTime > debounceTimer)
            {
                started = true;
            }
        }

        protected override void HandleHold(ActionEventArgs eventData)
        {
            if (started)
            {
                targetPitch = Mathf.Lerp(pitchMinMax.x, pitchMinMax.y, eventData.progress * 2);
                targetVolume = Mathf.Lerp(volumeMinMax.x, volumeMinMax.y, eventData.progress * 2);
            }
        }

        protected override void HandleEnd(ActionEventArgs eventData)
        {
            targetPitch = pitchMinMax.y;
            targetVolume = volumeMinMax.x;

            started = false;
            endTime = Time.time;
        }

        protected override void HandleCancel()
        {
            targetPitch = pitchMinMax.x;
            targetVolume = volumeMinMax.x;

            started = false;
            endTime = Time.time;
        }

        void Update()
        {
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * lerpSpeed);
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * lerpSpeed);
        }
    }
}
