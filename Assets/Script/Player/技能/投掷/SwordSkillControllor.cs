using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillControllor : SowrdSkill
{
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D collid;
    private float timer = 1f;
    private bool isReturning;
    private float returnSpeed = 3f;


    [Header("Bouncing剑")]
    private bool isBouncing;
    private int bounceAmount;
    [SerializeField] private List<Transform> enemyTarget;
    private int targetIndex;


    [Header("Pierce剑")]
    [SerializeField] private int pierceAmount;


    [Header("Spin剑")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float hitTimer;
    private float hitCooldown;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collid = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update

    // Update is called once per frame
    private new void Update()
    {
        timer -= Time.deltaTime;

        if ((timer < 0 || Input.GetKeyDown(KeyCode.Mouse1)) && !isSpinning)
        {
            ReturnSword();
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < .1f)
                player.CatchSword();
        }

        BounceLogic();  //弹跳逻辑

        SpinLogic();
    }





    //剑弹跳


    #region Regular剑
    public void SetupSword(Vector2 dir, float gravityScale, float facingDir)
    {
        if (isReturning == true)
            return;
        rb.gravityScale = gravityScale;
        rb.velocity = new Vector2(dir.x, dir.y);
        anim.SetBool("Rotate", true);
    }
    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        if (isSpinning)
        {
            StopWhileSpining();
            return;
        }
        //插入敌人身体后，剑停止
        collid.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (isBouncing == true && enemyTarget.Count > 0)
            return;
        anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }

    //返回剑
    public void ReturnSword()
    {
        rb.isKinematic = false;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        transform.parent = null;
        isReturning = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning == true)
            return;
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.StartCoroutine("FreezeTimerFor", 2f);  //击中敌人后，敌人短暂冻结
        }
        SetupTargetforBounce(collision);  //命中第一个敌人后，获取圈范围内的敌人
        StuckInto(collision);  //剑插入敌人身体
    }
    #endregion


    #region Spin剑
    private void StopWhileSpining()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhileSpining();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, .35f);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            hit.GetComponent<Enemy>().beAttack(-player.facingDir);
                        }
                    }
                }
            }
        }
    }
    #endregion


    #region Pierce剑
    public void SetupPierce(int _PierceAmount)
    {
        pierceAmount = _PierceAmount;
    }


    #endregion


    #region Bouncing剑
    public void SetupBounce(bool _isBouncing, int _amountOfBounce)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        if (isBouncing == true && enemyTarget.Count > 0)
        {
            targetIndex = 0;
            return;
        }
        if (enemyTarget.Count > 0)
            enemyTarget.Clear();
    }


    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, 5 * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                targetIndex++;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                }
                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void SetupTargetforBounce(Collider2D collision)
    {
        //处理弹跳逻辑
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().beAttack(-player.facingDir);
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }
    #endregion
}