using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private WheelConfigSO wheelConfig;

    private IZoneProgressionService _zoneService;
    private IRewardCollectionService _rewardService;
    private IGameStateMachine _stateMachine;
    private IWheelSpinResolver _spinResolver;

    private ZoneInfo _currentZone;

    public event Action<ZoneInfo> OnZoneChanged;
    public event Action<SpinResult> OnSpinResolved;
    public event Action<GameOutcome> OnRunEnded;

    [SerializeField] private WheelView wheelView;

    [SerializeField] private HudView hudView;

    private void Awake()
    {
        _zoneService = new ZoneProgressionService();
        _rewardService = new RewardCollectionService();
        _stateMachine = new GameStateMachine();
        _spinResolver = new WheelSpinResolver(new UnityRandomProvider());
    }

    private void Start()
    {
        hudView.OnSpinClicked += Spin;
        hudView.OnLeaveClicked += Leave;
        hudView.OnContinueClicked += ContinueToNextZone;
        hudView.OnRestartClicked += StartNewRun;

        OnRunEnded += outcome => hudView.ShowResultPanel(outcome, _rewardService.TotalValue);

        StartNewRun();
    }

    private void OnDestroy()
    {
        hudView.OnSpinClicked -= Spin;
        hudView.OnLeaveClicked -= Leave;
        hudView.OnContinueClicked -= ContinueToNextZone;
        hudView.OnRestartClicked -= StartNewRun;

        OnRunEnded -= outcome => hudView.ShowResultPanel(outcome, _rewardService.TotalValue);
    }

    public void StartNewRun()
    {
        _rewardService.ClearAll();
        _currentZone = _zoneService.GetZoneInfo(1);
        _stateMachine.Restart();
        RefreshWheelVisual();
        RefreshHud();
        OnZoneChanged?.Invoke(_currentZone);
    }

    public bool CanSpin() => _stateMachine.CanSpin();
    public bool CanLeave() => _stateMachine.CanLeave(_currentZone);

    public void Spin()
    {
        if (!CanSpin())
        {
            Debug.LogWarning("Spin requested but not allowed in current state.");
            return;
        }

        _stateMachine.RequestSpin();

        var slices = wheelConfig.GetSlicesForZoneType(_currentZone.Type);
        SpinResult result = _spinResolver.Resolve(slices);

        wheelView.PlaySpinAnimation(result.SliceIndex, slices.Count, () =>
        {
            ResolveSpinComplete(result);
        });
    }



    private void ResolveSpinComplete(SpinResult result)
    {
        _stateMachine.ResolveSpinResult(result.IsBomb);
        OnSpinResolved?.Invoke(result);

        if (result.IsBomb)
        {
            _rewardService.ClearAll();
            OnRunEnded?.Invoke(GameOutcome.BombHit);
            RefreshHud();
            return;
        }

        _rewardService.AddReward(result.Slice.reward, _currentZone.ZoneIndex);
        RefreshHud();
    }

    public void ContinueToNextZone()
    {
        if (_stateMachine.CurrentState != GameState.ResultResolved)
        {
            Debug.LogWarning("Cannot continue, no resolved result pending.");
            return;
        }

        _currentZone = _zoneService.Advance();
        _stateMachine.ContinueToNextZone();
        RefreshWheelVisual();
        RefreshHud();
        OnZoneChanged?.Invoke(_currentZone);
    }

    public void Leave()
    {
        if (!CanLeave())
        {
            Debug.LogWarning("Leave requested but not allowed in current state/zone.");
            return;
        }

        _stateMachine.RequestLeave();
        OnRunEnded?.Invoke(GameOutcome.CashedOut);
        RefreshHud();
    }

    private void RefreshWheelVisual()
    {
        var slices = wheelConfig.GetSlicesForZoneType(_currentZone.Type);
        wheelView.BuildSlices(slices);
        wheelView.SetZoneIndexText(_currentZone.ZoneIndex);
    }

    private void RefreshHud()
    {
        hudView.SetZoneIndex(_currentZone.ZoneIndex);
        hudView.SetTotalValue(_rewardService.TotalValue);

        bool isSpinning = _stateMachine.CurrentState == GameState.Spinning;
        bool canSpin = CanSpin();
        bool canLeave = CanLeave();
        bool isResultResolved = _stateMachine.CurrentState == GameState.ResultResolved;
        bool isGameOver = _stateMachine.CurrentState == GameState.GameOver;

        hudView.SetSpinInteractable(!isSpinning && canSpin);
        hudView.SetLeaveInteractable(!isSpinning && canLeave);
        hudView.SetContinueInteractable(!isSpinning && isResultResolved);

        hudView.ShowContinueButton(isResultResolved);
        hudView.ShowSpinButton(!isResultResolved && !isGameOver);
        hudView.ShowGameOverPanel(isGameOver);
    }

    public IReadOnlyList<CollectedReward> GetCollectedRewards() => _rewardService.CollectedRewards;
    public int GetTotalValue() => _rewardService.TotalValue;
    public ZoneInfo GetCurrentZone() => _currentZone;
}