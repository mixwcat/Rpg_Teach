using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterState
{
    private Enemy enemy;

    [Header("等级成长")]
    [SerializeField] private int level;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier;

    protected override void Start()
    {
        ApplyLevelModifiers();
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHp);
        Modify(armor);
        Modify(magicResistance);
        Modify(evasion);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }


    //每多一级，就可以自动生成一个Modifier
    private void Modify(Stats _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

}
