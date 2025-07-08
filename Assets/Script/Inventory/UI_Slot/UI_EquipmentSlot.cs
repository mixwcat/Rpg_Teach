using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType; // 装备类型

    public void OnValidate()
    {
        gameObject.name = "EquipmentSlot_" + equipmentType.ToString(); // 设置槽位名称为装备类型
    }

    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Inventory.instance.Unequip_Equipped_Item(item); // 调用Inventory的UnequipItem方法来卸下物品
        CleanUpSlot();
    }
}
