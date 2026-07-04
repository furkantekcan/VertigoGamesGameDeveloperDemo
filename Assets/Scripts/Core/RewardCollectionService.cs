using System;
using System.Collections.Generic;
using System.Linq;

public class RewardCollectionService : IRewardCollectionService
{
    private readonly List<CollectedReward> _collected = new();

    public IReadOnlyList<CollectedReward> CollectedRewards => _collected;
    public int TotalValue => _collected.Sum(r => r.Value); // System.Linq gerekli, ya da manuel toplama

    public event Action<CollectedReward> OnRewardCollected;
    public event Action OnRewardsCleared;

    public void AddReward(RewardSO reward, int zoneIndex)
    {
        int value = reward.GetValueForZone(zoneIndex);
        var collected = new CollectedReward(reward, value, zoneIndex);
        _collected.Add(collected);
        OnRewardCollected?.Invoke(collected);
    }

    public void ClearAll()
    {
        _collected.Clear();
        OnRewardsCleared?.Invoke();
    }
}