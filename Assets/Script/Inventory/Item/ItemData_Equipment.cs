using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon, //武器
    Armor, //护甲
    Flask //药水
}

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Item/EquipmentData")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType; //装备类型

    [Header("主要属性")]
    public int strength; //攻击力
    public int agility; //闪避率
    public int intelligence; //魔法攻击力
    public int vitality; //生命值

    [Header("攻击属性")]
    public int damage; //攻击力
    public int critChance; //暴击率
    public int critPower; //暴击伤害

    [Header("防御属性")]
    public int health; //生命值
    public int armor; //护甲值
    public int evasion; //闪避率
    public int magicResistance; //魔法抗性

    [Header("魔法属性")]
    public int fireDamage; //火焰伤害
    public int iceDamage; //冰霜伤害
    public int lightningDamage; //闪电伤害

    [Header("合成材料")]
    public List<InventoryItem> requiredMaterials; //合成所需材料列表


    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }
}
