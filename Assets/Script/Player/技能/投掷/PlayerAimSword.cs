using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSword : PlayerState
{
    public PlayerAimSword(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.instance.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.idleState);
        }
        base.Update();
        filpPlayer();
    }

    //瞄准时旋转
    private void filpPlayer()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < player.transform.position.x)
        {
            player.facingDir = -1;
        }
        else
        {
            player.facingDir = 1;
        }
    }
}