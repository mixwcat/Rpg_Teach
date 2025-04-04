using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  //class前面
public class Stats
{
    [SerializeField] private int baseValue;  //人物属性
    public List<int> modifiers;  //装备属性

    public int GetValue()   //最终属性值
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

    public void RemoveModifier(int modifier)
    {
        modifiers.RemoveAt(modifier);
    }
}