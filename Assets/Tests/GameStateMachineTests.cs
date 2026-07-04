using NUnit.Framework;

public class GameStateMachineTests
{
    private IGameStateMachine _sm;

    [SetUp]
    public void Setup()
    {
        _sm = new GameStateMachine();
    }

    [Test]
    public void InitialState_IsIdle()
    {
        Assert.AreEqual(GameState.Idle, _sm.CurrentState);
    }

    [Test]
    public void CannotLeave_WhenZoneIsNormal()
    {
        var normalZone = new ZoneInfo(1, ZoneType.Normal);
        Assert.IsFalse(_sm.CanLeave(normalZone));
    }

    [Test]
    public void CanLeave_WhenZoneIsSafe_AndIdle()
    {
        var safeZone = new ZoneInfo(5, ZoneType.Safe);
        Assert.IsTrue(_sm.CanLeave(safeZone));
    }

    [Test]
    public void CannotLeave_WhenSpinning_EvenIfSafeZone()
    {
        var safeZone = new ZoneInfo(5, ZoneType.Safe);
        _sm.RequestSpin();
        Assert.IsFalse(_sm.CanLeave(safeZone));
    }

    [Test]
    public void ResolveBomb_SetsGameOver()
    {
        _sm.RequestSpin();
        _sm.ResolveSpinResult(hitBomb: true);
        Assert.AreEqual(GameState.GameOver, _sm.CurrentState);
    }

    [Test]
    public void ResolveReward_SetsResultResolved()
    {
        _sm.RequestSpin();
        _sm.ResolveSpinResult(hitBomb: false);
        Assert.AreEqual(GameState.ResultResolved, _sm.CurrentState);
    }

    [Test]
    public void SpinWhileSpinning_Throws()
    {
        _sm.RequestSpin();
        Assert.Throws<System.InvalidOperationException>(() => _sm.RequestSpin());
    }
}