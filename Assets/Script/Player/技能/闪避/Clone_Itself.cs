using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Itself : CloneSkill
{
    //克隆体本身的行为
    [Header("克隆体基础属性")]
    private SpriteRenderer sr;  //克隆体的精灵
    [SerializeField] protected float colorLosingSpeed;
    [SerializeField] protected float cloneDuration;
    private float cloneTimer;

    [Header("克隆体目标")]
    private Transform closetEnemy;

    [Header("克隆体攻击")]
    private Animator anim;

    private void Awake()
    {
        cloneTimer = cloneDuration;
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Start()
    {
        anim.SetTrigger("Attack");
    }

    protected override void Update()
    {
        Clone_Dismiss();
    }



    private void Clone_Dismiss()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0 && sr != null)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));
        }
        if (sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    //面向敌人
    public void FaceClosetTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closetDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closetDistance)
                {
                    closetEnemy = hit.transform;
                }
            }
        }
        if (closetEnemy != null)
        {
            if (closetEnemy.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().beAttack(transform.localScale.x);
        }
    }
}