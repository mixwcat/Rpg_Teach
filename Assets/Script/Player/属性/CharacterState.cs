using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CharacterState : MonoBehaviour
{
    #region 属性
    [Header("血量")]
    public int currentHp;
    public Stats maxHp;  //最大血量
    public Image HealthIg;
    public System.Action onHealthChange; //血量变化事件


    [Header("伤害")]
    public Stats damage;
    public Stats critPower; //暴击伤害
    public Stats critChance; //暴击几率

    [Header("属性")]
    public Stats strength;  //力量，物理攻击
    public Stats agility;   //敏捷，速度
    public Stats intelligence;  //法力值，魔法伤害
    public Stats vitality;  //

    public Stats armor;   //护甲，物理防御
    public Stats evasion; //闪避
    public Stats magicResistance; //魔法抗性

    [Header("魔法属性")]
    public Stats fireDamage; //火焰伤害  
    public Stats iceDamage; //冰霜伤害
    public Stats lightningDamage; //光伤害

    public bool isIgnited; //是否被点燃
    public bool isChilled; //是否被冰冻
    public bool isShocked; //是否被电击

    private float ignitedTimer = 3; //点燃计时器
    private float ignitedDamageTimer = .3f; //点燃伤害计时器
    private float ignitedDamageCoolDown = .3f; //点燃伤害冷却时间
    #endregion  


    #region 基础脚本
    protected virtual void Start()
    {
        currentHp = maxHp.GetValue();
        critPower.SetDefaultValue(150);
    }

    protected virtual void Update()
    {
        //点燃状态
        ignitedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer <= 0 && isIgnited)
        {
            isIgnited = false;
        }

        if (ignitedDamageTimer <= 0 && isIgnited)
        {
            currentHp -= 10; //点燃伤害
            onHealthChange?.Invoke(); //触发血量变化事件
            ignitedDamageTimer = ignitedDamageCoolDown;
        }

    }
    #endregion

    #region 伤害计算
    //最终伤害计算
    public virtual void DoDamage(CharacterState _targetStats)
    {
        //闪避计算
        if (CanAvoidAttack(_targetStats))
            return;

        //冰冻减少防御
        int totalArmor = _targetStats.armor.GetValue() + _targetStats.vitality.GetValue();
        if (_targetStats.isChilled)
        {
            totalArmor = Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }

        //获取基础伤害
        int totalDamage = damage.GetValue() + strength.GetValue() - totalArmor;

        //暴击计算
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            return;
        }

        //造成伤害
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        //_targetStats.TakeDamage(totalDamage);
        DoMagicDamage(_targetStats);
    }

    //魔法伤害计算
    public virtual void DoMagicDamage(CharacterState _targetStats)
    {
        //获取基础魔法伤害
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightningDamage.GetValue();

        //属性伤害减去魔法抗性
        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue() - _targetStats.magicResistance.GetValue() - _targetStats.intelligence.GetValue() * 3;

        //造成伤害
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        _targetStats.TakeDamage(totalMagicDamage);

        //状态判断
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;
        if (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            canApplyIgnite = true;
        }
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    //buff管理
    public void ApplyAilments(bool _ignited, bool _chilled, bool _shocked)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }
        isIgnited = _ignited;
        if (isIgnited)
        {
            ignitedTimer = 3; //重置点燃计时器
            ignitedDamageTimer = ignitedDamageCoolDown; //重置点燃伤害计时器
        }
        isChilled = _chilled;
        isShocked = _shocked;
    }

    //闪避计算
    private bool CanAvoidAttack(CharacterState _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        //冰冻状态下，闪避率降低
        if (_targetStats.isChilled)
        {
            totalEvasion -= 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    //暴击概率计算
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) < totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    //暴击伤害计算
    private int CalculateCriticalDamage(int _damage)
    {
        float totalCriticalPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCriticalPower;
        return Mathf.RoundToInt(critDamage);
    }
    #endregion

    #region 基础功能
    //受伤
    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;

        onHealthChange?.Invoke(); //触发血量变化事件

        if (currentHp <= 0)
        {
            Die();
        }
    }

    //计算血量减少

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public int GetMaxHealthValue()
    {
        return maxHp.GetValue() + vitality.GetValue();
    }
    #endregion
}