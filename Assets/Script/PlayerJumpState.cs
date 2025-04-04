using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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
        player.rb.velocity = new Vector2(player.xInput * player.moveSpeed, player.rb.velocity.y);
        if (player.rb.velocity.y < 0)
            stateMachine.ChangeState(player.fallState);
    }
}