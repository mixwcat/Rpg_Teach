using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.attackOver = false;
        player.anim.speed = 10;
    }

    public override void Exit()
    {
        base.Exit();
        player.anim.speed = 1;
    }

    public override void Update()
    {
        base.Update();
        if (player.attackOver)
            stateMachine.ChangeState(player.idleState);
    }
}