using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cacophony;
using DG.Tweening;
using Leap;
using Leap.Attachments;
using UnityEngine.Animations;

public class HandyPadController : MonoBehaviour
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
    public bool isShown = false;
    private bool isAnimating = false;

    [Header("UI")]
    public Transform rootTransform;
    public Image leftSegment;
    public Image rightSegment;
    public Image upSegment;
    public Image downSegment;

    [Header("Particles")]
    public ParticleTrails particleTrails;
    public PositionConstraint positionConstraint;

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
    [Tooltip("Attachments hand are used to position the menu when summoned.")]
    public LeapHandConnector leapHandConnector;
    public Chirality activeChirality;
    public AttachmentHands attachmentHands;
    
    public Transform leftHandParent; // The canvas will be parented here for left hand
    public Transform rightHandParent; // The canvas will be parented here for right hand

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rootTransform == null)
        {
            rootTransform = transform;
        }

        showHideGesture.OnGestureEnd.AddListener(HandleShowHide);
        moveLeftGesture.OnGestureEnd.AddListener(HandleLeft);
        moveRightGesture.OnGestureEnd.AddListener(HandleRight);
        moveUpGesture.OnGestureEnd.AddListener(HandleUp);
        moveDownGesture.OnGestureEnd.AddListener(HandleDown);
        if (hideOnStart)
        {
            isShown = false;
            HideMenu(false);
        }
        if (attachmentHands == null)
        {
            attachmentHands = FindFirstObjectByType<AttachmentHands>();
        }
        leapHandConnector.OnNoHandPresentAfterTimeout.AddListener(HandleNoHandsAfterTimeout);
        leapHandConnector.OnHandChiralityChanged.AddListener(HandleHandChiralityChanged);
    }

    private void HandleNoHandsAfterTimeout()
    {
        HideMenu(false);
    }

    private void HandleHandChiralityChanged(Chirality chirality)
    {
        Debug.Log("HandleHandChiralityChanged:" + chirality);
        activeChirality = chirality;
        if (activeChirality == Chirality.Left)
        {
            // Move the menu to the Left...
            rootTransform.SetParent(leftHandParent, false);
            ConstraintSource constraint = new ConstraintSource(){ sourceTransform = leftHandParent, weight = 1f };
            positionConstraint.SetSource(0, constraint);

        }
        if (activeChirality == Chirality.Right)
        {
            // Move the menu to the Right...
            rootTransform.SetParent(rightHandParent, false);
            ConstraintSource constraint = new ConstraintSource() { sourceTransform = rightHandParent, weight = 1f };
            positionConstraint.SetSource(0, constraint);
        }
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
            isShown = true;
            OnMenuShown.Invoke();
            state = MenuState.IDLE;
        });
        if (interfaceAudio && playAudio)
        {
            PlayShowAudio();
        }
            attachmentHands.enabled = false;
        if (particleTrails != null)
        {
            particleTrails.DisableTrail();
        }
    }

    public void HideMenu(bool playAudio = true)
    {
        isAnimating = true;
        mainIndicator.transform.DOScale(0, 0.3f);
        mainIndicator.DOFade(0, 0.3f).OnComplete(() =>
        {
            isAnimating = false;
            isShown = false;
            // If NOT shown, reset the position...
            if (!isShown)
            {
                ResetMenuPosition();
            }
            OnMenuHidden.Invoke();
            state = MenuState.HIDDEN;
            attachmentHands.enabled = true;
        });
        if (interfaceAudio && playAudio)
        {
            PlayHideAudio();
        }
        if (particleTrails != null)
        {
            particleTrails.ActivateTrail();
        }
        
    }

    private bool CanDetect()
    {
        // Returns true if menu is active and not transitioning
        return (isShown && !isAnimating);
    }

    private void HandleShowHide()
    {
        // Toggle the isShown State and handle accordingly...
        isShown = !isShown;
        if (!isShown)
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

    public void Update()
    {
        // Check for active chirality...

    }
}
