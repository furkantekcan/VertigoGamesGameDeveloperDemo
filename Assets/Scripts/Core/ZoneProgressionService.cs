public class ZoneProgressionService : IZoneProgressionService
{
    private int _currentZoneIndex = 1;

    public int CurrentZoneIndex => _currentZoneIndex;

    public ZoneInfo GetZoneInfo(int zoneIndex)
    {
        ZoneType type = ResolveZoneType(zoneIndex);
        return new ZoneInfo(zoneIndex, type);
    }

    public ZoneInfo Advance()
    {
        _currentZoneIndex++;
        return GetZoneInfo(_currentZoneIndex);
    }

    private ZoneType ResolveZoneType(int zoneIndex)
{
    if (zoneIndex % 30 == 0) return ZoneType.Super;
    if (zoneIndex % 5 == 0) return ZoneType.Safe;
    return ZoneType.Normal;
}
}