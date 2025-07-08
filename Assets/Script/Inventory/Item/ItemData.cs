using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Material
}
[CreateAssetMenu(fileName = "NewItem", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType itemType; //物品类型
    public string itemName; //物品名称
    public Sprite icon; //物品图标
}
