using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WheelConfig_", menuName = "Wheel/Wheel Config")]
public class WheelConfigSO : ScriptableObject
{
    [Tooltip("Normal zonlarda kullanılan slice dizilimi (bomba dahil)")]
    public List<WheelSliceData> normalSlices = new();

    [Tooltip("Safe zone slice dizilimi (bomba YOK)")]
    public List<WheelSliceData> safeSlices = new();

    [Tooltip("Super zone slice dizilimi (bomba YOK, özel ödüller)")]
    public List<WheelSliceData> superSlices = new();

    public List<WheelSliceData> GetSlicesForZoneType(ZoneType zoneType)
    {
        return zoneType switch
        {
            ZoneType.Safe => safeSlices,
            ZoneType.Super => superSlices,
            _ => normalSlices
        };
    }
}