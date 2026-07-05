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
    public event Action OnGameOver;

    [SerializeField] private WheelView wheelView;

    private void Awake()
    {
        _zoneService = new ZoneProgressionService();
        _rewardService = new RewardCollectionService();
        _stateMachine = new GameStateMachine();
        _spinResolver = new WheelSpinResolver(new UnityRandomProvider());
    }

    private void Start()
    {
        StartNewRun();
    }

    public void StartNewRun()
    {
        _rewardService.ClearAll();
        _currentZone = _zoneService.GetZoneInfo(1);
        _stateMachine.Restart();
        OnZoneChanged?.Invoke(_currentZone);

        RefreshWheelVisual();
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
            OnGameOver?.Invoke();
            return;
        }

        _rewardService.AddReward(result.Slice.reward, _currentZone.ZoneIndex);
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
        OnZoneChanged?.Invoke(_currentZone);

        RefreshWheelVisual();
    }

    public void Leave()
    {
        if (!CanLeave())
        {
            Debug.LogWarning("Leave requested but not allowed in current state/zone.");
            return;
        }

        _stateMachine.RequestLeave();
        OnGameOver?.Invoke();
    }

    private void RefreshWheelVisual()
{
    var slices = wheelConfig.GetSlicesForZoneType(_currentZone.Type);
    wheelView.BuildSlices(slices);
    wheelView.SetZoneIndexText(_currentZone.ZoneIndex);
}

    public IReadOnlyList<CollectedReward> GetCollectedRewards() => _rewardService.CollectedRewards;
    public int GetTotalValue() => _rewardService.TotalValue;
    public ZoneInfo GetCurrentZone() => _currentZone;
}