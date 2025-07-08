using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Player : Entity
{
    public PlayerStateMachine stateMachine;

    #region 获取组件
    //[SerializeField] private Transform groundCheck;
    //[SerializeField] private LayerMask groundLayer;
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerAimSword aimSword { get; private set; }
    public PlayerCatchSword catchsword { get; private set; }
    public PlayerThrowSword throwSword { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerStats stats { get; private set; }

    #endregion 获取组件

    #region 基础属性

    public float xInput { get; private set; }

    public bool attackOver { get; set; }
    public GameObject sword { get; private set; }

    #endregion 基础属性

    #region 脚本数值

    [Header("移动")]
    public float moveSpeed;

    [Header("跳跃")]
    public float jumpForce;

    //[SerializeField] private float groundCheckDistance;

    [Header("打击感")]
    public float shakeTime;

    public int attackPause;
    public float attackStrength;

    [Header("冲刺")]
    public float dashSpeed;

    public float dashDuration;

    public float dashTime;

    #endregion 脚本数值

    protected override void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        attackState = new PlayerAttackState(stateMachine, this, "Attack");
        jumpState = new PlayerJumpState(stateMachine, this, "Jump");
        fallState = new PlayerFallState(stateMachine, this, "Jump");
        aimSword = new PlayerAimSword(stateMachine, this, "aimSword");
        throwSword = new PlayerThrowSword(stateMachine, this, "throwSword");
        catchsword = new PlayerCatchSword(stateMachine, this, "catchSword");
        dashState = new PlayerDashState(stateMachine, this, "Dash");
        blackHoleState = new PlayerBlackHoleState(stateMachine, this, "Jump");
    }

    protected override void Start()
    {
        base.Start();
        stats = GetComponent<PlayerStats>();
        stateMachine.Initialized(idleState);
        facingDir = 1;
        //mainCamera = Camera.main;
    }

    protected override void Update()
    {
        stateMachine.currentState.Update();
        xInput = Input.GetAxisRaw("Horizontal");

        if (xInput != 0)
            facingDir = xInput;

        if (Input.GetKeyDown(KeyCode.G))
        {
            SkillManager.instance.crystal.UseSkill();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStats _target = other.GetComponent<EnemyStats>();
            other.GetComponent<Enemy>().beAttack(facingDir);
            AttackEffect.Instance.HitEffect(attackPause, shakeTime, attackStrength);
            stats.DoDamage(_target);
        }
    }



    public void AssignSword(GameObject sword) => this.sword = sword;

    public void CatchSword()
    {
        stateMachine.ChangeState(catchsword);
        Destroy(sword);
    }

    public void ExitBlackHole()
    {
        stateMachine.ChangeState(fallState);
        rb.gravityScale = 1;
    }
}