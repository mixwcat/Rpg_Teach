using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region ��ȡ���

    public Rigidbody2D rb;
    public Animator anim;
    public Animator hitEffect;
    public Transform playerDetected;
    public LayerMask playerLayer;
    public float playerCheckDistance;
    public GameObject redEffect;

    #endregion ��ȡ���

    #region ��������

    private float enemySpeed = .5f;
    public float miniPosi;
    public float maxPosi;
    private Vector2 direction;
    public float timer;
    private EnemyFx EnemyFx;
    private bool isBusy;
    public CharacterState stats { get; private set; }

    #endregion ��������

    private void Start()
    {
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

    private void SetEffectTrue() => redEffect.SetActive(true);

    private void SetEffectFalse()
    {
        redEffect.SetActive(false);
    }
}