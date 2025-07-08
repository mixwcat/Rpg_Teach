using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrystalSkillControllor : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>(); //动画控制器
    private CircleCollider2D cd => GetComponent<CircleCollider2D>(); //圆形碰撞器
    private float crystalExistTimer;
    public bool canExplode; //水晶是否可以爆炸
    public bool canMove;
    [SerializeField] private float moveSpeed; //水晶移动速度

    private bool canGrow; //水晶是否可以生长
    private float growSpeed = 1; //水晶生长速度

    public void SetupCrystal(float _crystalDuration)
    {
        crystalExistTimer = _crystalDuration; //设置水晶存在时间
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime; //每帧减少水晶存在时间
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(2, 2), growSpeed * Time.deltaTime); //水晶生长
        }
        if (canMove) //如果水晶可以移动
        {
            MoveToClosetEnemy(); //水晶移动到最近的敌人位置
            if (Vector2.Distance(transform.position, FindClosetEnemy(transform).position) < .1f) //如果水晶和最近的敌人距离小于0.5
            {
                canMove = false; //水晶停止移动
                FinishCrystal(); //调用结束水晶方法
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode) //如果水晶可以爆炸
        {
            canGrow = true; //允许水晶生长
            canMove = false; //水晶停止移动
            anim.SetTrigger("Explode"); //调用爆炸方法
        }
        else //如果水晶不能爆炸
        {
            SelfDestroy(); //调用自毁方法
        }
    }

    public void AnimationExplode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius); //获取所有在水晶范围内的碰撞体
        foreach (Collider2D collider in colliders) //遍历所有碰撞体
        {
            if (collider.CompareTag("Enemy")) //如果碰撞体是敌人
            {
                Enemy enemy = collider.GetComponent<Enemy>(); //获取敌人组件
                enemy.beAttack(1); //对敌人造成伤害
            }
        }
    }

    public void DestroyCrystalEvent()
    {
        SelfDestroy(); //调用自毁方法
    }

    public void SelfDestroy()
    {
        Destroy(gameObject); //销毁水晶实例
    }

    private void MoveToClosetEnemy()
    {
        transform.position = Vector2.MoveTowards(transform.position, FindClosetEnemy(transform).position, moveSpeed * Time.deltaTime); //水晶移动到最近的敌人位置
    }

    private Transform FindClosetEnemy(Transform crystalTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(crystalTransform.position, 10); //获取所有在水晶范围内的碰撞体
        float closestDistance = 10; //最近的距离
        Transform closestEnemy = null; //最近的敌人
        foreach (Collider2D collider in colliders) //遍历所有碰撞体
        {
            if (collider.CompareTag("Enemy")) //如果碰撞体是敌人
            {
                float distance = Vector2.Distance(crystalTransform.position, collider.transform.position); //计算水晶和敌人之间的距离
                if (distance < closestDistance) //如果距离小于最近的距离
                {
                    closestEnemy = collider.transform; //更新最近的敌人
                }
            }
        }
        return closestEnemy; //返回最近的敌人
    }
}
