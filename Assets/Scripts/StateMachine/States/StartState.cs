using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : State
{
    public StartState(PlayerController player, StateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void Enter()
    {

        // CameraManager.Instance.SetLive("startCam");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.ChangeState(StateEnum.PlayState);
        }


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
