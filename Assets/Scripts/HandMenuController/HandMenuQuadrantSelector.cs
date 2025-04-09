using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cacophony;
using DG.Tweening;

public class HandMenuQuadrantSelector : MonoBehaviour
{
    /// <summary>
    ///  Grab Release summons and dismisses the menu.
    ///  Pinch + Move will trigger the four directional axis inputs.
    /// </summary>
    ///
    public enum MenuState { HIDDEN, IDLE, LEFT, RIGHT, UP, DOWN };
    public MenuState state;
    public GestureConsumerUnityEvents showHideGesture;
    public GestureConsumerUnityEvents moveLeftGesture;
    public GestureConsumerUnityEvents moveRightGesture;
    public GestureConsumerUnityEvents moveUpGesture;
    public GestureConsumerUnityEvents moveDownGesture;
    public Image posIndicator;
    public Vector2 moveDistance = new(108, 108);

    public CanvasGroup mainIndicator;
    public CanvasGroup arrowIndicators;
    public bool hideOnStart;
    [HideInInspector]
    private bool _isShown = false;
    public bool IsShown { get { return _isShown; } }
    private bool isAnimating = false;

    [Header("UI")]
    public Transform rootTransform;
    public Image leftSegment;
    public Image rightSegment;
    public Image upSegment;
    public Image downSegment;

    [Header("Audio")]
    public bool interfaceAudio = true;
    public AudioSource audioSource;
    public AudioClip showMenuAudio;
    public AudioClip hideMenuAudio;
    public AudioClip moveOptionAudio;
    public AudioClip selectedOptionAudio;

    [HideInInspector] public UnityEvent OnLeftFocused = new();
    [HideInInspector] public UnityEvent OnLeftSelected = new();
    [HideInInspector] public UnityEvent OnRightFocused = new();
    [HideInInspector] public UnityEvent OnRightSelected = new();
    [HideInInspector] public UnityEvent OnUpFocused = new();
    [HideInInspector] public UnityEvent OnUpSelected = new();
    [HideInInspector] public UnityEvent OnDownFocused = new();
    [HideInInspector] public UnityEvent OnDownSelected = new();
    [HideInInspector] public UnityEvent OnMenuShown = new();
    [HideInInspector] public UnityEvent OnMenuHidden = new();

    [Header("Hands")]
    public LeapHandConnector leapHandConnector;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rootTransform == null)
        {
            rootTransform = transform;
        }

        showHideGesture.OnGestureHoldWithPosition.AddListener(SetMenuPosition);
        showHideGesture.OnGestureEnd.AddListener(HandleShowHide);
        moveLeftGesture.OnGestureEnd.AddListener(HandleLeft);
        moveRightGesture.OnGestureEnd.AddListener(HandleRight);
        moveUpGesture.OnGestureEnd.AddListener(HandleUp);
        moveDownGesture.OnGestureEnd.AddListener(HandleDown);
        if (hideOnStart)
        {
            _isShown = false;
            HideMenu(false);
        }

        leapHandConnector.OnNoHandPresentAfterTimeout.AddListener(HandleNoHandsAfterTimeout);
    }

    private void HandleNoHandsAfterTimeout()
    {
        HideMenu(false);
    }

    private void PlayShowAudio()
    {
        audioSource.PlayOneShot(showMenuAudio);
    }

    private void PlayHideAudio()
    {
        audioSource.PlayOneShot(hideMenuAudio);
    }

    private void PlayMoveAudio()
    {
        audioSource.PlayOneShot(moveOptionAudio);
    }

    private void PlaySelectedAudio()
    {
        audioSource.PlayOneShot(selectedOptionAudio);
    }

    public void ShowMenu(bool playAudio = true)
    {
        isAnimating = true;
        mainIndicator.transform.DOScale(1, 0.3f);
        mainIndicator.transform.DOPunchRotation(new Vector3(60,0,0), 0.5f, 0, 0.01f);
        mainIndicator.DOFade(1, 0.3f).OnComplete(() =>
        {
            isAnimating = false;
            _isShown = true;
            OnMenuShown.Invoke();
            state = MenuState.IDLE;
        });
        if (interfaceAudio && playAudio)
        {
            PlayShowAudio();
        }
    }

    public void HideMenu(bool playAudio = true)
    {
        isAnimating = true;
        mainIndicator.transform.DOScale(0, 0.3f);
        mainIndicator.DOFade(0, 0.3f).OnComplete(() =>
        {
            isAnimating = false;
            _isShown = false;
            // If NOT shown, reset the position...
            if (!IsShown)
            {
                ResetMenuPosition();
            }
            OnMenuHidden.Invoke();
            state = MenuState.HIDDEN;
        });
        if (interfaceAudio && playAudio)
        {
            PlayHideAudio();
        }
    }

    private bool CanDetect()
    {
        // Returns true if menu is active and not transitioning
        return (IsShown && !isAnimating);
    }

    private void SetMenuPosition(Vector3 arg0)
    {
        if (!IsShown)
        {
            // Set the menu position, offset in z from the hand...
            Vector3 handPos = arg0;
            rootTransform.position = handPos + new Vector3(0, 0, 0.5f);
        }
    }

    private void HandleShowHide()
    {
        // Toggle the isShown State and handle accordingly...
        _isShown = !IsShown;
        if (!IsShown)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }

    public void ResetMenuPosition()
    {
        posIndicator.rectTransform.DOAnchorPos(new Vector2(0, 0f), 0f);
    }

    private void HandleLeft()
    {
        if (CanDetect())
        {
            isAnimating = true;
            if (state != MenuState.LEFT)
            {
                PlayMoveAudio();
                leftSegment.rectTransform.DOPunchAnchorPos(new Vector2(-5, 0), 0.5f, 1, 0.01f);
                posIndicator.rectTransform.DOAnchorPos(new Vector2(-moveDistance.x, 0f), 0.3f).OnComplete(() =>
                {
                    isAnimating = false;
                    OnLeftFocused.Invoke();
                    state = MenuState.LEFT;
                });
            }
            else
            {
                PlaySelectedAudio();
                OnLeftSelected.Invoke();
                leftSegment.rectTransform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.2f).OnComplete(() =>
                {
                    HideMenu(false);
                    leftSegment.rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
                });
            }
        }
    }
    private void HandleRight()
    {
        if (CanDetect())
        {
            isAnimating = true;
            if (state != MenuState.RIGHT)
            {
                PlayMoveAudio();
                rightSegment.rectTransform.DOPunchAnchorPos(new Vector2(5, 0), 0.2f, 1, 0.01f);
                posIndicator.rectTransform.DOAnchorPos(new Vector2(moveDistance.x, 0f), 0.3f).OnComplete(() =>
                {
                    isAnimating = false;
                    OnRightFocused.Invoke();
                    state = MenuState.RIGHT;
                });
            }
            else
            {
                PlaySelectedAudio();
                OnRightSelected.Invoke();
                rightSegment.rectTransform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.2f).OnComplete(() =>
                {
                    HideMenu(false);
                    rightSegment.rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
                });
            }
        }
    }

    private void HandleUp()
    {
        if (CanDetect())
        {
            isAnimating = true;
            if (state != MenuState.UP)
            {
                PlayMoveAudio();
                upSegment.rectTransform.DOPunchAnchorPos(new Vector2(0, 5), 0.5f, 1, 0.01f);
                posIndicator.rectTransform.DOAnchorPos(new Vector2(0f, moveDistance.y), 0.5f).OnComplete(() =>
                {
                    isAnimating = false;
                    OnUpFocused.Invoke();
                    state = MenuState.UP;
                });
            }
            else
            {
                PlaySelectedAudio();
                OnUpSelected.Invoke();
                upSegment.rectTransform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.2f).OnComplete(() =>
                {
                    HideMenu(false);
                    upSegment.rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
                });
            }
        }
    }

    private void HandleDown()
    {
        if (CanDetect())
        {
            isAnimating = true;
            if (state != MenuState.DOWN)
            {
                PlayMoveAudio();
                downSegment.rectTransform.DOPunchAnchorPos(new Vector2(0, -5), 0.5f, 1, 0.01f);
                posIndicator.rectTransform.DOAnchorPos(new Vector2(0f, -moveDistance.y), 0.5f).OnComplete(() =>
                {
                    isAnimating = false;
                    OnDownFocused.Invoke();
                    state = MenuState.DOWN;
                });
            }
            else
            {
                PlaySelectedAudio();
                OnDownSelected.Invoke();
                downSegment.rectTransform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.2f).OnComplete(() =>
                {
                    HideMenu(false);
                    downSegment.rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
                });
            }
        }
    }

}
