using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSword : PlayerState
{
    private Transform sword;
    public PlayerCatchSword(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (!player.IsGroundDetected())
        {
            return;
        }
        sword = player.sword.transform;
        if (player.transform.position.x < sword.position.x)
        {
            player.facingDir = 1;
        }
        else
        {
            player.facingDir = -1;
        }
        swordShock();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    private void swordShock()
    {
        player.rb.velocity = new Vector2(3 * -player.facingDir, 3);
    }
}