using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("碰撞检测")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] protected float wallCheckDistance;

    #region 获取组件

    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    #endregion 获取组件

    #region 基础属性

    public float facingDir { get; set; }

    #endregion

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {

    }

    public void Flip()
    {
        facingDir = -facingDir;
        transform.localScale = new Vector3(facingDir, 1, 1);
    }


    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

    //public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wallLayer);

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance);
        //Gizmos.DrawRay(wallCheck.position, Vector2.right * wallCheckDistance);
    }
}
