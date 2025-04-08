using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using System.Collections.Generic;

public class HandAudioMixer : MonoBehaviour
{
    public enum StemTrack {  UP,DOWN,LEFT,RIGHT }
    public AudioMixer mixer;
    public AudioSource upStem;
    public AudioSource downStem;
    public AudioSource leftStem;
    public AudioSource rightStem;
    private List<AudioMixerGroup> mixerGroups;
    public StemTrack startupStem = StemTrack.UP;
    public StemTrack activeStem;
    private AudioSource activeAudioSource = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoloStem(startupStem);
        upStem.loop = true;
        downStem.loop = true;
        leftStem.loop = true;
        rightStem.loop = true;
    }

    public void SoloStem(StemTrack track)
    {
        // Crossfade out the old channel...
        if (activeAudioSource != null)
        {
            string volumeParm = "";
            switch (activeStem)
            {
                case StemTrack.UP:
                    volumeParm = "UpVolume";
                    break;
                case StemTrack.DOWN:
                    volumeParm = "DownVolume";
                    break;
                case StemTrack.LEFT:
                    volumeParm = "LeftVolume";
                    break;
                case StemTrack.RIGHT:
                    volumeParm = "RightVolume";
                    break;
            }
            activeAudioSource.outputAudioMixerGroup.audioMixer.DOSetFloat(volumeParm, -80f, 0.5f).OnComplete(() =>
            {
                activeAudioSource.Stop();
            });
        }

        switch (track)
        {
            case StemTrack.UP:
                Debug.Log("Starting UP track");
                upStem.Play();
                upStem.outputAudioMixerGroup.audioMixer.DOSetFloat("UpVolume", 0f, 0.5f);
                activeStem = StemTrack.UP;
                activeAudioSource = upStem;
                break;
            case StemTrack.DOWN:
                downStem.Play();
                downStem.outputAudioMixerGroup.audioMixer.DOSetFloat("DownVolume", 0f, 0.5f);
                activeStem = StemTrack.DOWN;
                activeAudioSource = downStem;
                break;
            case StemTrack.LEFT:
                leftStem.Play();
                leftStem.outputAudioMixerGroup.audioMixer.DOSetFloat("LeftVolume", 0f, 0.5f);
                activeStem = StemTrack.LEFT;
                activeAudioSource = leftStem;
                break;
            case StemTrack.RIGHT:
                rightStem.Play();
                rightStem.outputAudioMixerGroup.audioMixer.DOSetFloat("RightVolume", 0f, 0.5f);
                activeStem = StemTrack.RIGHT;
                activeAudioSource = rightStem;
                break;
        }
    }

    public void MuteAll()
    {
        foreach (AudioMixerGroup group in mixerGroups)
        {
            group.audioMixer.DOSetFloat("MasterVolume", -80f, 0.25f);
        }
    }
}
