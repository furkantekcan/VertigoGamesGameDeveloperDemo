using System;

public class GameStateMachine : IGameStateMachine
{
    private GameState _currentState = GameState.Idle;

    public GameState CurrentState => _currentState;
    public event Action<GameState> OnStateChanged;

    public bool CanSpin()
    {
        return _currentState == GameState.Idle;
    }

    public bool CanLeave(ZoneInfo currentZone)
    {
        // Kural: sadece spin dönmüyorken (Idle) VE zon safe/super ise leave edilebilir
        return _currentState == GameState.Idle && currentZone.AllowsLeave;
    }

    public void RequestSpin()
    {
        if (!CanSpin())
        {
            throw new InvalidOperationException($"Cannot spin while state is {_currentState}");
        }
        SetState(GameState.Spinning);
    }

    public void ResolveSpinResult(bool hitBomb)
    {
        if (_currentState != GameState.Spinning)
        {
            throw new InvalidOperationException($"Cannot resolve result while state is {_currentState}");
        }
        SetState(hitBomb ? GameState.GameOver : GameState.ResultResolved);
    }

    public void RequestLeave()
    {
        // Not: CanLeave zone bilgisi istediği için burada sadece state kontrolü yapılır,
        // zone kontrolü çağıran taraf (GameController) tarafından CanLeave ile önceden yapılmalı
        if (_currentState != GameState.Idle && _currentState != GameState.ResultResolved)
        {
            throw new InvalidOperationException($"Cannot leave while state is {_currentState}");
        }
        SetState(GameState.GameOver); // "leave" de oyunu bitirir, ödüller commit edilmiş olur
    }

    public void Restart()
    {
        SetState(GameState.Idle);
    }

    private void SetState(GameState newState)
    {
        _currentState = newState;
        OnStateChanged?.Invoke(newState);
    }

    public void ContinueToNextZone()
{
    if (_currentState != GameState.ResultResolved)
    {
        throw new InvalidOperationException($"Cannot continue while state is {_currentState}");
    }
    SetState(GameState.Idle);
}
}