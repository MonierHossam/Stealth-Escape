using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PopUp : MonoBehaviour
{
    [SerializeField] GameObject parentScreen;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private Ease hideEase = Ease.InBack;
    [SerializeField] private float startScale = 0.5f;

    [Header("Fade Settings")]
    [SerializeField] private bool useFade = true;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Events")]
    public UnityEvent OnShowComplete;
    public UnityEvent OnHideComplete;

    private RectTransform rectTransform;
    private Sequence currentAnimation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (useFade && canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
    }

    public void Show()
    {
        if (parentScreen != null)
        {
            parentScreen.SetActive(true);
        }

        KillCurrentAnimation();
        gameObject.SetActive(true);
        currentAnimation = DOTween.Sequence();

        // Set initial state
        rectTransform.localScale = Vector3.one * startScale;
        if (useFade)
        {
            canvasGroup.alpha = 0f;
        }

        // Scale animation
        currentAnimation.Append(rectTransform.DOScale(Vector3.one, animationDuration).SetEase(showEase));

        // Fade animation
        if (useFade)
        {
            currentAnimation.Join(canvasGroup.DOFade(1f, animationDuration).SetEase(Ease.OutQuad));
        }

        currentAnimation.OnComplete(() => OnShowComplete?.Invoke());

    }

    public void Hide()
    {
        KillCurrentAnimation();
        currentAnimation = DOTween.Sequence();

        // Scale animation
        currentAnimation.Append(rectTransform.DOScale(Vector3.one * startScale, animationDuration).SetEase(hideEase));

        // Fade animation
        if (useFade)
        {
            currentAnimation.Join(canvasGroup.DOFade(0f, animationDuration).SetEase(Ease.InQuad));
        }

        currentAnimation.OnComplete(() =>
        {
            if (parentScreen != null)
            {
                parentScreen.SetActive(false);
            }

            gameObject.SetActive(false);
            OnHideComplete?.Invoke();
        });
    }

    private void KillCurrentAnimation()
    {
        if (currentAnimation != null && currentAnimation.IsActive())
        {
            currentAnimation.Kill();
        }
    }

    private void OnDisable()
    {
        KillCurrentAnimation();
    }

    private void OnDestroy()
    {
        KillCurrentAnimation();
    }
}
