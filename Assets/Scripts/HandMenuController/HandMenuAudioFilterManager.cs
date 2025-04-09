using UnityEngine;
using System.Collections.Generic;

public class HandMenuAudioFilterManager : MonoBehaviour
{
    /// <summary>
    /// Takes events from HandMenuQuadrantSelector, plays an audio track
    /// and applies one of four filters to the audio
    /// </summary>
    public HandMenuQuadrantSelector handPadController;
    public enum FilterDirection {UP,DOWN,LEFT,RIGHT}
    public AudioSource audioSource;
    public AudioHighPassFilter upFilter;
    public AudioLowPassFilter downFilter;
    public AudioDistortionFilter leftFilter;
    public AudioEchoFilter rightFilter;

    private Dictionary<FilterDirection, Behaviour> filterComponentDict;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        handPadController.OnUpFocused.AddListener(delegate { SoloFilter(FilterDirection.UP); });
        handPadController.OnDownFocused.AddListener(delegate { SoloFilter(FilterDirection.DOWN); });
        handPadController.OnLeftFocused.AddListener(delegate { SoloFilter(FilterDirection.LEFT); });
        handPadController.OnRightFocused.AddListener(delegate { SoloFilter(FilterDirection.RIGHT); });

        filterComponentDict = new()
        {
            { FilterDirection.UP, upFilter },
            { FilterDirection.DOWN, downFilter },
            { FilterDirection.LEFT, leftFilter },
            { FilterDirection.RIGHT, rightFilter },
        };
    }

    public void SoloFilter(FilterDirection filter)
    {
        foreach (FilterDirection direction in filterComponentDict.Keys)
        {
            filterComponentDict[direction].enabled = (direction == filter);
        } 
    }
}
