using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;   // 物品图标
    [SerializeField] private TextMeshProUGUI itemText;     // 物品数量文本

    public InventoryItem item; // 当前物品的InventoryItem

    public void Awake()
    {
        itemImage = GetComponent<Image>();
        itemText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        if (_newItem == null || _newItem.data == null)
            Debug.LogWarning("尝试更新物品槽时传入的物品无效或数据为空。");
        itemImage.sprite = _newItem.data.icon; // 设置物品图标
        itemText.text = _newItem.stackSize.ToString(); // 设置物品数量
        itemImage.color = Color.white; // 确保图标可见
        item = _newItem; // 更新当前物品
    }


    public void CleanUpSlot()
    {
        itemImage.sprite = null; // 清除物品图标
        itemText.text = ""; // 清除物品数量文本
        itemImage.color = new Color(0, 0, 0, 0); // 隐藏图标
        item = null; // 清除当前物品
    }


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.UnequipItem(item); // 如果是装备，调用装备方法
        }
    }

}
