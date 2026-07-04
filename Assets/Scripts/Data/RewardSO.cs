using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Wheel/Reward")]
public class RewardSO : ScriptableObject
{
    public string rewardId;
    public Sprite icon;
    public int baseValue;
    [Tooltip("Zon arttıkça değerin nasıl büyüyeceğini belirler")]
    public AnimationCurve valueGrowthCurve = AnimationCurve.Linear(0, 1, 100, 3);

    public int GetValueForZone(int zoneIndex)
    {
        float multiplier = valueGrowthCurve.Evaluate(zoneIndex);
        return Mathf.RoundToInt(baseValue * multiplier);
    }
}