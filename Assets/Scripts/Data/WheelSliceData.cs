using UnityEngine;

[System.Serializable]
public class WheelSliceData
{
    public SliceType sliceType;
    public RewardSO reward;      // sliceType == Reward ise dolu olmalı
    public Sprite sliceIcon;     // görsel override (opsiyonel, reward.icon kullanılabilir)
}