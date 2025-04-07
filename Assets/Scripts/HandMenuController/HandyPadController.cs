using UnityEngine;
using Cacophony;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class HandyPadController : MonoBehaviour
{
    /// <summary>
    ///  Grab Release summons and dismisses the menu.
    ///  Pinch + Move will trigger the four directional axis inputs.
    /// </summary>
    ///
    public GestureConsumerUnityEvents showHideGesture;
    public GestureConsumerUnityEvents moveLeftGesture;
    public GestureConsumerUnityEvents moveRightGesture;
    public GestureConsumerUnityEvents moveUpGesture;
    public GestureConsumerUnityEvents moveDownGesture;
    public GestureConsumerUnityEvents confirmGesture;
    public Image posIndicator;
    public Vector2 moveDistance = new(108, 108);

    public CanvasGroup mainIndicator;
    public CanvasGroup arrowIndicators;
    public bool hideOnStart;
    public bool isShown = false;
    private bool isAnimating = false;

    [Header("Audio")]
    public bool interfaceAudio = true;
    public AudioSource audioSource;
    public AudioClip showMenuAudio;
    public AudioClip hideMenuAudio;
    public AudioClip moveOptionAudio;

    [Header("Events")]
    public UnityEvent OnLeftSelected;
    public UnityEvent OnRightSelected;
    public UnityEvent OnUpSelected;
    public UnityEvent OnDownSelected;
    public UnityEvent OnMenuShown;
    public UnityEvent OnMenuHidden;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        showHideGesture.OnGestureEnd.AddListener(HandleShowHide);
        moveLeftGesture.OnGestureEnd.AddListener(HandleLeft);
        moveRightGesture.OnGestureEnd.AddListener(HandleRight);
        moveUpGesture.OnGestureEnd.AddListener(HandleUp);
        moveDownGesture.OnGestureEnd.AddListener(HandleDown);
        confirmGesture.OnGestureEnd.AddListener(HandleConfirm);
        if (hideOnStart)
        {
            isShown = false;
            HideMenu(false);
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

    public void ShowMenu(bool playAudio = true)
    {
        isAnimating = true;
        mainIndicator.transform.DOScale(1, 0.3f);
        mainIndicator.DOFade(1, 0.3f).OnComplete(() =>
        {
            isAnimating = false;
            isShown = true;
            OnMenuShown.Invoke();
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
            isShown = false;
            Debug.Log("HideMenu: isShown:" + isShown);
            // If NOT shown, reset the position...
            if (!isShown)
            {
                ResetMenuPosition();
            }
            OnMenuHidden.Invoke();
        });
        if (interfaceAudio && playAudio)
        {
            PlayHideAudio();
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
        Debug.Log("HandleShowHide, isShownState should be: " + isShown);
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

    private void HandleConfirm()
    {
        Debug.Log("CONFIRM");
    }

    private void HandleLeft()
    {
        if (CanDetect())
        {
            isAnimating = true;
            PlayMoveAudio();
            posIndicator.rectTransform.DOAnchorPos(new Vector2(-moveDistance.x, 0f), 0.3f).OnComplete(() =>
            {
                isAnimating = false;
                OnLeftSelected.Invoke();
            });
        }
    }
    private void HandleRight()
    {
        if (CanDetect())
        {
            isAnimating = true;
            PlayMoveAudio();
            posIndicator.rectTransform.DOAnchorPos(new Vector2(moveDistance.x, 0f), 0.3f).OnComplete(() =>
            {
                isAnimating = false;
                OnRightSelected.Invoke();
            });
        }
    }

    private void HandleUp()
    {
        if (CanDetect())
        {
            isAnimating = true;
            PlayMoveAudio();
            posIndicator.rectTransform.DOAnchorPos(new Vector2(0f, moveDistance.y), 0.5f).OnComplete(() =>
            {
                isAnimating = false;
                OnUpSelected.Invoke();
            });
        }
    }

    private void HandleDown()
    {
        if (CanDetect())
        {
            isAnimating = true;
            PlayMoveAudio();
            posIndicator.rectTransform.DOAnchorPos(new Vector2(0f, -moveDistance.y), 0.5f).OnComplete(() =>
            {
                isAnimating = false;
                OnDownSelected.Invoke();
            });
        }
    }
}
