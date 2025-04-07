using UnityEngine;
using Cacophony;
using UnityEngine.UI;
using DG.Tweening;

public class HandyPadController : MonoBehaviour
{
    /// <summary>
    ///  Double Tap Summons and dismisses the control.
    ///  Pinch+Move will trigger the four directional axis inputs.
    /// </summary>
    ///
    public GestureConsumerUnityEvents showHideGesture;
    public GestureConsumerUnityEvents moveLeftGesture;
    public GestureConsumerUnityEvents moveRightGesture;
    public GestureConsumerUnityEvents moveUpGesture;
    public GestureConsumerUnityEvents moveDownGesture;
    public GestureConsumerUnityEvents confirmGesture;
    public Image posIndicator;
    public Vector2 moveDistance = new(100, 100);

    public CanvasGroup mainCanvas;
    public bool isShown = false;
    public bool isAnimating = false;

    private bool CanDetect()
    {
        // Returns true if menu is active and not transitioning
        return (isShown && !isAnimating);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        showHideGesture.OnGestureEnd.AddListener(HandleShowHide);
        moveLeftGesture.OnGestureEnd.AddListener(HandleLeft);
        moveRightGesture.OnGestureEnd.AddListener(HandleRight);
        moveUpGesture.OnGestureEnd.AddListener(HandleUp);
        moveDownGesture.OnGestureEnd.AddListener(HandleDown);
        confirmGesture.OnGestureEnd.AddListener(HandleConfirm);
    }

    private void HandleShowHide()
    {
        Debug.Log("DOUBLE TAP");
        float fadeValue = isShown ? 0f : 1f;
        float scaleValue = isShown ? 0f : 1f;
        isAnimating = true;
        mainCanvas.transform.DOScale(scaleValue, 0.3f);
        mainCanvas.DOFade(fadeValue, 0.3f).OnComplete(() =>
        {
            isAnimating = false;
            isShown = !isShown;
            // If NOT shown, reset the position...
            if (!isShown)
            {
                ResetMenuPosition();
            }
        });
    }

    public void ResetMenuPosition()
    {
        posIndicator.rectTransform.DOAnchorPos(new Vector2(0, 0f), 0f);
    }

    private void HandleConfirm()
    {
        Debug.Log("CONFIRM");
    }

    private void HandleLeft()
    {
        if (CanDetect())
        {
            Debug.Log("LEFT");
            isAnimating = true;
            posIndicator.rectTransform.DOAnchorPos(new Vector2(-moveDistance.x, 0f), 0.3f).OnComplete(() =>
            {
                isAnimating = false;
            });
        }
    }
    private void HandleRight()
    {
        if (CanDetect())
        {
            Debug.Log("RIGHT");
            isAnimating = true;
            posIndicator.rectTransform.DOAnchorPos(new Vector2(moveDistance.x, 0f), 0.3f).OnComplete(() =>
            {
                isAnimating = false;
            });
        }
    }

    private void HandleUp()
    {
        if (CanDetect())
        {
            Debug.Log("UP");
            isAnimating = true;
            posIndicator.rectTransform.DOAnchorPos(new Vector2(0f, moveDistance.y), 0.5f).OnComplete(() =>
            {
                isAnimating = false;
            });
        }
    }

    private void HandleDown()
    {
        if (CanDetect())
        {
            Debug.Log("DOWN");
            isAnimating = true;
            posIndicator.rectTransform.DOAnchorPos(new Vector2(0f, -moveDistance.y), 0.5f).OnComplete(() =>
            {
                isAnimating = false;
            });
        }
    }
}
