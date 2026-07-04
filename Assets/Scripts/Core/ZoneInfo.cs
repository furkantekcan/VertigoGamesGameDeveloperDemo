using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct ZoneInfo
{
    public readonly int ZoneIndex;
    public readonly ZoneType Type;

    public ZoneInfo(int zoneIndex, ZoneType type)
    {
        ZoneIndex = zoneIndex;
        Type = type;
    }

    public bool AllowsLeave => Type != ZoneType.Normal;
    public bool HasBomb => Type == ZoneType.Normal;
}
