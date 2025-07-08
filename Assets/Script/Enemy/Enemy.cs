using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 获取组件
    public Rigidbody2D rb;
    public Animator anim;
    public EnemyStateMachine stateMachine;
    #endregion


    #region 检测玩家
    public Transform playerDetected;
    public LayerMask playerLayer;
    public float playerCheckDistance;
    #endregion


    #region 受击特效
    public Animator hitEffect;
    public GameObject redEffect;
    #endregion


    #region 基础属性
    private float enemySpeed = .5f;
    private float defaultSpeed = .5f;
    public float miniPosi;
    public float maxPosi;
    private Vector2 direction;
    public float timer;
    private EnemyFx EnemyFx;
    private bool isBusy;
    public CharacterState stats { get; private set; }
    #endregion 基础属性

    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
    }


    private void Start()
    {
        //stateMachine.currentState.Update();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hitEffect = transform.Find("Effect").GetComponent<Animator>();
        direction = Vector2.right;
        miniPosi += transform.position.x;
        maxPosi += transform.position.x;
        EnemyFx = GetComponent<EnemyFx>();
        stats = GetComponent<CharacterState>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isBusy = false;
            moveAndTurn();
            rb.velocity = new Vector2(direction.x * enemySpeed, rb.velocity.y);
        }

        if (IsPlayerDetected() && !isBusy)
        {
            anim.SetTrigger("Attack");
            rb.velocity = Vector2.zero;
        }
    }

    public bool IsPlayerDetected() => Physics2D.Raycast(playerDetected.position, Vector2.right * direction, playerCheckDistance, playerLayer);

    public void beAttack(float facingDir)
    {
        isBusy = true;
        timer = 1;
        anim.SetTrigger("BeAttack");
        hitEffect.SetTrigger("BeAttack");
        transform.localScale = new Vector3(-facingDir, 1, 1);
        EnemyFx.StartCoroutine("FlashFx");
        rb.velocity = new Vector2(facingDir, 1);
    }

    public void moveAndTurn()
    {
        anim.SetBool("Move", true);

        if (transform.position.x > maxPosi)
        {
            direction = Vector2.left;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (transform.position.x < miniPosi)
        {
            direction = Vector2.right;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer = .3f;
            if (other.transform.position.x > transform.position.x)
            {
                direction = Vector2.right;
                transform.localScale = new Vector3(1, 1, 1);
                rb.velocity = new Vector2(direction.x * enemySpeed, rb.velocity.y);
            }
            else
            {
                direction = Vector2.left;
                transform.localScale = new Vector3(-1, 1, 1);
                rb.velocity = new Vector2(direction.x * enemySpeed, rb.velocity.y);
            }
        }
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            enemySpeed = 0;
            anim.speed = 0;
        }
        else
        {
            enemySpeed = defaultSpeed;
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    private void SetEffectTrue() => redEffect.SetActive(true);

    private void SetEffectFalse()
    {
        redEffect.SetActive(false);
    }
}