using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  //classǰ��
public class Stats
{
    [SerializeField] private int baseValue;  //��������
    public List<int> modifiers;  //װ������

    public int GetValue()   //��������ֵ
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