using NUnit.Framework;

public class ZoneProgressionServiceTests
{
    private IZoneProgressionService _service;

    [SetUp]
    public void Setup()
    {
        _service = new ZoneProgressionService();
    }

    [Test]
    public void Zone1_IsNormal()
    {
        var info = _service.GetZoneInfo(1);
        Assert.AreEqual(ZoneType.Normal, info.Type);
        Assert.IsTrue(info.HasBomb);
        Assert.IsFalse(info.AllowsLeave);
    }

    [Test]
    public void Zone5_IsSafe()
    {
        var info = _service.GetZoneInfo(5);
        Assert.AreEqual(ZoneType.Safe, info.Type);
        Assert.IsTrue(info.AllowsLeave);
        Assert.IsFalse(info.HasBomb);
    }

    [Test]
    public void Zone30_IsSuper_NotSafe()
    {
        var info = _service.GetZoneInfo(30);
        Assert.AreEqual(ZoneType.Super, info.Type);
    }

    [Test]
    public void Zone10_IsSafe()
    {
        var info = _service.GetZoneInfo(10);
        Assert.AreEqual(ZoneType.Safe, info.Type);
    }

    [Test]
    public void Advance_IncrementsZoneIndex()
    {
        _service.GetZoneInfo(1); // no-op, sadece current başlangıcı 1
        var next = _service.Advance();
        Assert.AreEqual(2, next.ZoneIndex);
    }
}
