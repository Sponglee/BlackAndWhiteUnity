using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : State
{


    public PauseState(PlayerController playerController, StateMachine stateMachine) : base(playerController, stateMachine)
    {
        //Maybe initialize joystick here and disabling it later?
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


        // CameraManager.Instance.SetLive("playCam");
        // CameraManager.Instance.AssignFollowTarget(_playerController.transform);
        // CameraManager.Instance.AssignLookAtTarget(_playerController.transform);
        // base.Enter();
    }

    public override void Exit()
    {

        base.Exit();
    }

    public override void HandleInput()
    {

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {

        // _playerController.Move(new Vector3(joystick.Horizontal, 0f, joystick.Vertical));
        base.PhysicsUpdate();
    }

}
