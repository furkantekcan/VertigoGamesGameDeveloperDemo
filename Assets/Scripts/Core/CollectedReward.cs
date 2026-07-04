public readonly struct CollectedReward
{
    public readonly RewardSO Reward;
    public readonly int Value;
    public readonly int ZoneIndex;

    public CollectedReward(RewardSO reward, int value, int zoneIndex)
    {
        Reward = reward;
        Value = value;
        ZoneIndex = zoneIndex;
    }
}