using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    protected bool triggerCalled;
    private string animBoolName;

    protected float stateTimer;

    public EnemyState(EnemyStateMachine _stateMachine, Enemy _enemy, string _animBoolName)
    {
        this.animBoolName = _animBoolName;
        this.stateMachine = _stateMachine;
        this.enemy = _enemy;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName, false);
    }
}
