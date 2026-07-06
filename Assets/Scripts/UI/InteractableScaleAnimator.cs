using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class InteractableScaleAnimator : UIComponentBase
{
    [Header("Auto-Bound References")]
    [SerializeField] private Button button;
    [SerializeField] private RectTransform targetScaleTransform; // ui_animContainer_button_scale gibi ayrı bir transform

    [Header("Animation Settings")]
    [SerializeField] private float enableScale = 1f;
    [SerializeField] private float disableScale = 0f;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private Ease enableEase = Ease.OutBack;
    [SerializeField] private Ease disableEase = Ease.InBack;

    private bool _lastInteractable;
    private bool _initialized;

    protected override void AutoBindReferences()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (targetScaleTransform == null)
        {
            targetScaleTransform = GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (!_initialized || button.interactable != _lastInteractable)
        {
            AnimateToState(button.interactable);
            _lastInteractable = button.interactable;
            _initialized = true;
        }
    }
    private void AnimateToState(bool interactable)
    {
        if (targetScaleTransform == null) return;

        float target = interactable ? enableScale : disableScale;
        Ease ease = interactable ? enableEase : disableEase;

        targetScaleTransform.DOKill();
        targetScaleTransform.DOScale(target, duration).SetEase(ease);
    }
}