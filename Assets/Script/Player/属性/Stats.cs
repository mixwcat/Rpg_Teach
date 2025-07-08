using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    //人物基础血量和装备血量相加
    [SerializeField] private int baseValue;  //人物属性
    public List<int> modifiers;  //装备属性

    public int GetValue()   //人物属性和装备属性相加
    {
        int finalValue = baseValue;
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue;
    }

    public void AddModifier(int modifier)
    {
        modifiers.Add(modifier);
    }

    public void SetDefaultValue(int value)
    {
        baseValue = value;
    }

    public void RemoveModifier(int modifier)
    {
        modifiers.Remove(modifier);
    }
}