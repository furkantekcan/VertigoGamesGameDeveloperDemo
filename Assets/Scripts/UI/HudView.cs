using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudView : UIComponentBase
{
    [Header("Auto-Bound References")]
    [SerializeField] private Button spinButton;
    [SerializeField] private Button leaveButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Text totalValueText;
    [SerializeField] private TMP_Text zoneIndexText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject bombResultContent;
    [SerializeField] private GameObject cashOutResultContent;
    [SerializeField] private TMP_Text finalValueText;

    public event Action OnSpinClicked;
    public event Action OnLeaveClicked;
    public event Action OnContinueClicked;
    public event Action OnRestartClicked;

    protected override void AutoBindReferences()
    {
    }

    private void OnEnable()
    {
        spinButton.onClick.AddListener(HandleSpinClicked);
        leaveButton.onClick.AddListener(HandleLeaveClicked);
        continueButton.onClick.AddListener(HandleContinueClicked);
        restartButton.onClick.AddListener(HandleRestartClicked);
    }

    private void OnDisable()
    {
        spinButton.onClick.RemoveListener(HandleSpinClicked);
        leaveButton.onClick.RemoveListener(HandleLeaveClicked);
        continueButton.onClick.RemoveListener(HandleContinueClicked);
        restartButton.onClick.RemoveListener(HandleRestartClicked);
    }

    public void ShowResultPanel(GameOutcome outcome, int totalValue)
    {
        gameOverPanel.SetActive(outcome != GameOutcome.None);

        bool isBomb = outcome == GameOutcome.BombHit;
        bombResultContent.SetActive(isBomb);
        cashOutResultContent.SetActive(!isBomb && outcome == GameOutcome.CashedOut);

        if (!isBomb && finalValueText != null)
            finalValueText.text = totalValue.ToString();
    }

    private void HandleSpinClicked() => OnSpinClicked?.Invoke();
    private void HandleLeaveClicked() => OnLeaveClicked?.Invoke();
    private void HandleContinueClicked() => OnContinueClicked?.Invoke();
    private void HandleRestartClicked() => OnRestartClicked?.Invoke();

    public void SetSpinInteractable(bool interactable) => spinButton.interactable = interactable;
    public void SetLeaveInteractable(bool interactable) => leaveButton.interactable = interactable;
    public void SetContinueInteractable(bool interactable) => continueButton.interactable = interactable;

    public void ShowContinueButton(bool show) => continueButton.gameObject.SetActive(show);
    public void ShowSpinButton(bool show) => spinButton.gameObject.SetActive(show);

    public void SetTotalValue(int value) => totalValueText.text = "You lost all your rewards!";
    public void SetZoneIndex(int zoneIndex) => zoneIndexText.text = zoneIndex.ToString();

    public void ShowGameOverPanel(bool show) => gameOverPanel.SetActive(show);
}