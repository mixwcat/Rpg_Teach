using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.sword)
            stateMachine.ChangeState(player.aimSword);
        if (Input.GetKeyDown(KeyCode.J))
            stateMachine.ChangeState(player.attackState);
        if (Input.GetKeyDown(KeyCode.Space) && (player.IsGroundDetected()))
        {
            player.rb.velocity = new Vector2(player.xInput, player.jumpForce);
            stateMachine.ChangeState(player.jumpState);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }
}