using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : State
{
    [SerializeField] Joystick joystick;
    [SerializeField] GameObject joystickSystem;


    public PlayState(PlayerController playerController, StateMachine stateMachine) : base(playerController, stateMachine)
    {
        //Maybe initialize joystick here and disabling it later?
    }

    public override void Enter()
    {

        //Set up joystick as Input System
        if (joystickSystem == null)
        {
            GameObject joystickPrefab = Resources.Load("Prefabs/JoystickSystem") as GameObject;
            joystickSystem = GameObject.Instantiate(joystickPrefab);
            joystick = joystickSystem.GetComponentInChildren<Joystick>();
        }


        CameraManager.Instance.SetLive("playCam");
        CameraManager.Instance.AssignFollowTarget(_playerController.transform);
        CameraManager.Instance.AssignLookAtTarget(_playerController.transform);
        base.Enter();
    }

    public override void Exit()
    {
        //Dispose of joystick System on state exit
        GameObject.Destroy(joystickSystem);
        base.Exit();
    }

    public override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.ChangeState(StateEnum.StartState);
        }


    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {

        _playerController.Move(new Vector3(joystick.Horizontal, 0f, joystick.Vertical));
        base.PhysicsUpdate();
    }

}
