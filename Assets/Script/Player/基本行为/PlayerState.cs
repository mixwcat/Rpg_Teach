using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    private string animBoolName;

    public PlayerState(PlayerStateMachine stateMachine, Player player, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    // Start is called before the first frame update
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        player.anim.SetFloat("yVelocity", player.rb.velocity.y);
        player.transform.localScale = new Vector3(player.facingDir, 1, 1);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}