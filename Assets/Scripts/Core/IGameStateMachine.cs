using System;

public interface IGameStateMachine
{
    GameState CurrentState { get; }
    event Action<GameState> OnStateChanged;

    bool CanSpin();
    bool CanLeave(ZoneInfo currentZone);

    void RequestSpin();
    void ResolveSpinResult(bool hitBomb);
    void RequestLeave();
    void Restart();
    void ContinueToNextZone();
}