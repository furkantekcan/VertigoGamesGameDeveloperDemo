using System;
using System.Collections.Generic;

public interface IRewardCollectionService
{
    IReadOnlyList<CollectedReward> CollectedRewards { get; }
    int TotalValue { get; }

    event Action<CollectedReward> OnRewardCollected;
    event Action OnRewardsCleared;

    void AddReward(RewardSO reward, int zoneIndex);
    void ClearAll();      // bomba durumunda
}