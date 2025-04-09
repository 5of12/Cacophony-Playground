using System;
using Cacophony;
using UnityEngine;

public class HandMenuParticlesController : MonoBehaviour
{
    public HandMenuQuadrantSelector handMenu;
    public LeapHandConnector leapHandConnector;

    [Header("Particles")]
    public ParticleTrails particleTrails;
    public Gradient upGradient;
    public Gradient downGradient;
    public Gradient leftGradient;
    public Gradient rightGradient;
    public int minParticles = 5;
    public int maxParticles = 500;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handMenu.OnMenuShown.AddListener(HandleMenuShown);
        handMenu.OnMenuHidden.AddListener(HandleMenuHidden);
        handMenu.OnUpFocused.AddListener(HandleUp);
        handMenu.OnDownFocused.AddListener(HandleDown);
        handMenu.OnLeftFocused.AddListener(HandleLeft);
        handMenu.OnRightFocused.AddListener(HandleRight);
        leapHandConnector.OnNewData.AddListener(HandleHandData);
        leapHandConnector.OnHandFound.AddListener(HandleHandAppeared);
        leapHandConnector.OnNoHandPresentAfterTimeout.AddListener(HandleHandLost);
    }

    private void HandleHandAppeared()
    {
        particleTrails.ActivateTrail();
        particleTrails.EmitRate = maxParticles;
    }

    private void HandleHandLost()
    {
        particleTrails.DisableTrail();
    }

    private void HandleMenuShown()
    {
        if (particleTrails != null)
        {
            particleTrails.EmitRate = minParticles;
        }
    }

    private void HandleMenuHidden()
    {
        if (particleTrails != null)
        {
            particleTrails.EmitRate = maxParticles;
        }
    }

    private void HandleLeft()
    {
        particleTrails.SetColor(leftGradient);
    }

    private void HandleRight()
    {
        particleTrails.SetColor(rightGradient);
    }

    private void HandleUp()
    {
        particleTrails.SetColor(upGradient);
    }

    private void HandleDown()
    {
        particleTrails.SetColor(downGradient);
    }

    private void HandleHandData(HandDataEventArgs handData)
    {
        Vector3 position = handData.handPosition + handData.handPose.palmDirection * 0.1f;
        particleTrails.SetPosition(position, !handMenu.IsShown);
    }
}
