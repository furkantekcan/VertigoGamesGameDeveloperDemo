public interface IZoneProgressionService
{
    ZoneInfo GetZoneInfo(int zoneIndex);
    ZoneInfo Advance(); // bir sonraki zona geçer, ZoneInfo döner
    int CurrentZoneIndex { get; }
}
