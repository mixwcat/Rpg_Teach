using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.instance.clone.CreateClone(player.transform);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.rb.velocity = new Vector2(player.moveSpeed * player.facingDir * player.dashSpeed, 0);
        player.dashTime += Time.deltaTime;
        if (player.dashTime >= player.dashDuration)
        {
            player.dashTime = 0;
            stateMachine.ChangeState(player.idleState);
            player.rb.velocity = Vector2.zero;
        }
    }
}