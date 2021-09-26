public abstract class State
{
    #region Constructor
    protected State(PlayerController player, StateMachine stateMachine)
    {
        _playerController = player;
        _stateMachine = stateMachine;
    }
    #endregion

    #region Fields
    protected PlayerController _playerController;
    protected StateMachine _stateMachine;
    #endregion

    public virtual void Enter()
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}