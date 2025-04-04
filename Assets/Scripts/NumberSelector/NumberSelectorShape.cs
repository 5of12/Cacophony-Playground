using Cacophony;
using UnityEngine;

public class NumberSelectorShape : MonoBehaviour
{
    public NumberSelectorShapeGenerator shapeGenerator;
    public GestureConsumerUnityEvents gestureConsumer;
    public int numTargets = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        gestureConsumer.OnGestureHoldWithProgress.AddListener(OnGestureStart);
        gestureConsumer.OnGestureEnd.AddListener(OnGestureEnd);
        gestureConsumer.OnGestureCancel.AddListener(OnGestureEnd);
    }

    private void OnDisable()
    {
        gestureConsumer.OnGestureHoldWithProgress.RemoveListener(OnGestureStart);
        gestureConsumer.OnGestureEnd.RemoveListener(OnGestureEnd);
    }

    private void OnGestureStart(float progress)
    {
        if (progress < 0.2f)
        {
            shapeGenerator.SetTargetCount(numTargets, progress);
        }
    }

    private void OnGestureEnd()
    {
        shapeGenerator.ReleaseInfluence(numTargets);
    }

    
}
