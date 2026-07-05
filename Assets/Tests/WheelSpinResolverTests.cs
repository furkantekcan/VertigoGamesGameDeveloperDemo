using System.Collections.Generic;
using NUnit.Framework;

public class WheelSpinResolverTests
{
    private List<WheelSliceData> BuildTestSlices()
    {
        return new List<WheelSliceData>
        {
            new WheelSliceData { sliceType = SliceType.Reward },
            new WheelSliceData { sliceType = SliceType.Reward },
            new WheelSliceData { sliceType = SliceType.Bomb },
            new WheelSliceData { sliceType = SliceType.Reward },
        };
    }

    [Test]
    public void Resolve_ReturnsBomb_WhenFixedIndexPointsToBomb()
    {
        var resolver = new WheelSpinResolver(new FakeRandomProvider(fixedIndex: 2));
        var result = resolver.Resolve(BuildTestSlices());

        Assert.IsTrue(result.IsBomb);
        Assert.AreEqual(2, result.SliceIndex);
    }

    [Test]
    public void Resolve_ReturnsReward_WhenFixedIndexPointsToReward()
    {
        var resolver = new WheelSpinResolver(new FakeRandomProvider(fixedIndex: 0));
        var result = resolver.Resolve(BuildTestSlices());

        Assert.IsFalse(result.IsBomb);
    }

    [Test]
    public void Resolve_EmptyList_Throws()
    {
        var resolver = new WheelSpinResolver(new FakeRandomProvider(fixedIndex: 0));
        Assert.Throws<System.ArgumentException>(() => resolver.Resolve(new List<WheelSliceData>()));
    }

    [Test]
    public void RequiredSliceCount_Is8()
    {
        Assert.AreEqual(8, WheelConfigSO.RequiredSliceCount);
    }
}