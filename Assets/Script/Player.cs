using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine;

    #region 获取组件

    public Rigidbody2D rb;
    public Animator anim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerAimSword aimSword { get; private set; }
    public PlayerCatchSword catchsword { get; private set; }
    public PlayerThrowSword throwSword { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public CharacterState stats { get; private set; }

    #endregion 获取组件

    #region 基础属性

    public float xInput;
    public float facingDir;
    public bool attackOver { get; set; }
    public GameObject sword;

    #endregion 基础属性

    #region 脚本数值

    [Header("移动")]
    public float moveSpeed;

    [Header("跳跃")]
    public float jumpForce;

    [SerializeField] private float groundCheckDistance;

    [Header("打击感")]
    public float shakeTime;

    public int attackPause;
    public float attackStrength;

    [Header("冲刺")]
    public float dashSpeed;

    public float dashDuration;

    public float dashTime;

    #endregion 脚本数值

    public void Awake()
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
    }

    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterState>();
        stateMachine.Initialized(idleState);
        facingDir = 1;
    }

    public void Update()
    {
        stateMachine.currentState.Update();
        xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0)
            facingDir = xInput;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().beAttack(facingDir);
            AttackSence.Instance.HitPause(attackPause);
            AttackSence.Instance.ShakeCamera(shakeTime, attackStrength);
            other.GetComponent<CharacterState>().TakeDamage(stats.damage.GetValue());
        }
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

    private void OnDrawGizmos() => Gizmos.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance);

    public void AssignSword(GameObject sword) => this.sword = sword;

    public void ClearSword() => Destroy(sword);
}