using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : State
{
    // [SerializeField] Joystick joystick;
    // [SerializeField] GameObject joystickSystem;


    public PlayState(PlayerController playerController, StateMachine stateMachine) : base(playerController, stateMachine)
    {
        //Maybe initialize joystick here and disabling it later?
        // _playerController = playerController;


    }

    public override void Enter()
    {

        // //Set up joystick as Input System
        // if (joystickSystem == null)
        // {
        //     GameObject joystickPrefab = Resources.Load("Prefabs/JoystickSystem") as GameObject;
        //     joystickSystem = GameObject.Instantiate(joystickPrefab);
        //     joystick = joystickSystem.GetComponentInChildren<Joystick>();
        // }


        CameraManager.Instance.SetLive("playCam");
        CameraManager.Instance.AssignFollowTarget(_playerController.transform);
        CameraManager.Instance.AssignLookAtTarget(_playerController.transform);
        base.Enter();
    }

    public override void Exit()
    {
        //Dispose of joystick System on state exit
        // GameObject.Destroy(joystickSystem);
        base.Exit();
    }

    public override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.ChangeState(StateEnum.StartState);
        }

        if (Input.GetAxis("Fire1") != 0)
        {

            _playerController.Attack();
        }

        _playerController.Move(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {


        base.PhysicsUpdate();
    }

}
