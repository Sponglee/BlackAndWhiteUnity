using System.Collections;
using UnityEngine;
using UnityEngine.Events;




public class GameManager : Singleton<GameManager>
{
    #region Events
    public class GameFinishedEvent : UnityEvent<bool> { };
    public static GameFinishedEvent OnGameFinished = new GameFinishedEvent();
    public class MenuOpenEvent : UnityEvent<bool> { };
    public static MenuOpenEvent OnMenuOpen = new MenuOpenEvent();
    #endregion

    #region private
    [SerializeField] private StateEnum currentState;
    [SerializeField] private GameObject playerPrefab;
    #endregion

    #region public 
    public StateEnum CurrentState { get; set; }
    public StateMachine StateMachine { get; private set; }
    public StartState StartState { get; private set; }
    public PlayState PlayState { get; private set; }
    public FinishState FinishState { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public bool IsEnabled { get; private set; }
    #endregion

    #region properties
    #endregion

    #region UnityCalls

    private void Awake()
    {
        // Initializing state machine with all possible states
        StateMachine = new StateMachine();

        //Initialize the player
        PlayerController = Instantiate(playerPrefab).GetComponent<PlayerController>();

        StartState = new StartState(PlayerController, StateMachine);
        PlayState = new PlayState(PlayerController, StateMachine);
        FinishState = new FinishState(PlayerController, StateMachine);

    }

    private void Start()
    {
        StateMachine.OnStateChange += GameStateHandler;

        // Applying default state in state machine
        StateMachine.Initialize(GetState(currentState));
    }

    private void Update()
    {
        // Handling current state update calls
        StateMachine.CurrentState.HandleInput();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        // Handling current state fixed update calls
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion


    public void ChangeState(StateEnum value)
    {
        currentState = value;
        StateMachine.ChangeState(GetState(value));
    }

    private State GetState(StateEnum defaultState)
    {
        State state = null;

        switch (defaultState)
        {
            case StateEnum.StartState:
                state = StartState;
                break;
            case StateEnum.PlayState:
                state = PlayState;
                break;
            case StateEnum.FinishState:
                state = FinishState;
                break;
        }
        return state;
    }


    private void GameStateHandler(object sender, State targetState)
    {
        switch (targetState)
        {
            case StartState _:
                {
                    break;
                }
            case PlayState _:
                {
                    break;
                }
            case FinishState _:
                {
                    break;
                }
        }
    }
}
