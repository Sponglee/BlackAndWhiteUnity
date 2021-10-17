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
    public PauseState PauseState { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public Transform spawnPoint;
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
        if (PlayerController == null)
        {
            PlayerController = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<PlayerController>();
            PlayerController.gameObject.SetActive(false);
        }

        StartState = new StartState(PlayerController, StateMachine);
        PlayState = new PlayState(PlayerController, StateMachine);
        FinishState = new FinishState(PlayerController, StateMachine);
        PauseState = new PauseState(PlayerController, StateMachine);

    }

    private void Start()
    {
        StateMachine.OnStateChange += GameStateHandler;

        // Applying default state in state machine
        StateMachine.Initialize(StateEnumToState(CurrentState));
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

    public void GameOver()
    {

    }



    public void ChangeState(StateEnum value)
    {
        // Debug.Log("STATE CHANGED TO " + value);
        CurrentState = value;
        StateMachine.ChangeState(StateEnumToState(value));

    }

    public StateEnum CheckState()
    {
        State state = StateMachine.CurrentState;

        StateEnum stateEnum = StateEnum.PauseState;

        switch (state)
        {
            case StartState _:
                {
                    stateEnum = StateEnum.StartState;
                    // PlayerController.SetUpPath(StageController.Instance.currentPath);
                    break;
                }
            case PlayState _:
                {
                    stateEnum = StateEnum.PlayState;
                    break;
                }
            case FinishState _:
                {
                    stateEnum = StateEnum.FinishState;
                    break;
                }
        }

        return stateEnum;
    }

    private State StateEnumToState(StateEnum defaultState)
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
            case StateEnum.PauseState:
                state = PauseState;
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
            case PauseState _:
                {
                    break;
                }
            case PlayState _:
                {
                    PlayerController.gameObject.SetActive(true);

                    break;
                }
            case FinishState _:
                {
                    break;
                }
        }
    }
}
