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
    public GestureConsumerUnityEvents doubleTapGesture;
    public GestureConsumerUnityEvents moveLeftGesture;
    public GestureConsumerUnityEvents moveRightGesture;
    public GestureConsumerUnityEvents moveUpGesture;
    public GestureConsumerUnityEvents moveDownGesture;
    public Image posIndicator;
    public Vector2 moveDistance = new(100, 100);

    public CanvasGroup mainCanvas;
    public bool isShown = false;
    private bool isAnimating = false;

    private bool CanDetect()
    {
        // Returns true if menu is active and not transitioning
        return (isShown && !isAnimating);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doubleTapGesture.OnGestureEnd.AddListener(HandleDoubleTap);
        moveLeftGesture.OnGestureEnd.AddListener(HandleLeft);
        moveRightGesture.OnGestureEnd.AddListener(HandleRight);
        moveUpGesture.OnGestureEnd.AddListener(HandleUp);
        moveDownGesture.OnGestureEnd.AddListener(HandleDown);
    }

    private void HandleDoubleTap()
    {
        Debug.Log("DOUBLE TAP");
        
        float fadeValue = isShown ? 0f : 1f;
        isAnimating = true;
        mainCanvas.DOFade(fadeValue, 0.3f).OnComplete(() =>
        {
            isAnimating = false;
            isShown = !isShown;
            Debug.Log("New Value = " + isShown);
        });
    }

    private void HandleLeft()
    {
        if (CanDetect())
        {
            Debug.Log("LEFT");
            posIndicator.rectTransform.DOAnchorPos(new Vector2(-moveDistance.x, 0f), 0.3f);
        }
    }
    private void HandleRight()
    {
        Debug.Log("RIGHT");
        posIndicator.rectTransform.DOAnchorPos(new Vector2(moveDistance.x, 0f), 0.3f);
    }

    private void HandleUp()
    {
        Debug.Log("UP");
        posIndicator.rectTransform.DOAnchorPos(new Vector2(0f, moveDistance.y), 0.5f);
    }

    private void HandleDown()
    {
        Debug.Log("DOWN");
        posIndicator.rectTransform.DOAnchorPos(new Vector2(0f, -moveDistance.y), 0.5f);
    }
}
