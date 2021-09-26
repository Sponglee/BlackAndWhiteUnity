using System;

public class StateMachine
{
    #region Events
    public EventHandler<State> OnStateChange;
    #endregion

    public State CurrentState { get; private set; }

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
        OnStateChange?.Invoke(this, CurrentState);
    }

    public void ChangeState(State newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();

        OnStateChange?.Invoke(this, CurrentState);
    }
}
