using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float stateTimer;

    public PlayerBlackHoleState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }



    public override void Enter()
    {
        base.Enter();
        skillUsed = false;
        stateTimer = flyTime;
        player.rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;
        if (stateTimer > 0)
        {
            player.rb.velocity = new Vector2(0, 3);
        }
        else
        {
            player.rb.velocity = new Vector2(0, -(.1f));

            if (!skillUsed)
            {
                SkillManager.instance.blackHole.UseSkill();
                skillUsed = true;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
