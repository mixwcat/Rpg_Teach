using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region 单例模式
    public static Inventory instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 保持在场景切换时不销毁
        }
        else
        {
            Destroy(gameObject); // 确保只有一个实例存在
        }
    }
    #endregion


    #region 三种背包定义

    [Header("背包父位置")]
    //武器背包
    [SerializeField] private Transform inventorySlotParent; // 背包UI的父物体
    private UI_ItemSlot[] itemSlot;   //在parent下的Slot预制件

    // 材料背包
    [SerializeField] private Transform materialSlotParent; // 材料背包UI的父物体
    private UI_ItemSlot[] materialSlot;   //材料背包parent下的Slot预制件

    // 已装备武器的背包
    [SerializeField] private Transform equippedSlotParent; // 已装备武器的UI的父物体
    private UI_EquipmentSlot[] equippedSlot;   //已装备武器的parent下的Slot预制件



    [Header("item列表和字典")]
    // Inventory里存放的不是Item，而是（Item+stack），封装在InventoryItem中
    public List<InventoryItem> inventoryItems;  //装备背包
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;  //物品 映射 （物品+数量）


    public List<InventoryItem> materialItems; //材料背包
    public Dictionary<ItemData, InventoryItem> materialDictionary; // 材料物品 映射 （物品+数量）


    public List<InventoryItem> equippedItems; // 已装备武器的背包
    public Dictionary<ItemData, InventoryItem> equippedDictionary; // 已装备武器 映射 （物品+数量）

    //为什么要用字典？：碰撞传入的是ItemData，无法得知这个item的数量。所以需要用字典来映射物品和数量的关系

    #endregion

    private void Start()
    {
        // 初始化背包数据
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        // 初始化材料背包数据
        materialItems = new List<InventoryItem>();
        materialDictionary = new Dictionary<ItemData, InventoryItem>();

        // 初始化已装备武器数据
        equippedItems = new List<InventoryItem>(); // 初始时没有装备的武器
        equippedDictionary = new Dictionary<ItemData, InventoryItem>();

        // 创建了几个预制件，获取几个
        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        materialSlot = materialSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equippedSlot = equippedSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }


    #region 更新，添加和删除背包物品

    public void Unequip_Equipped_Item(InventoryItem item)
    {
        // 添加到普通背包
        AddItem(item.data);

        // 从已装备武器的背包中移除
        equippedDictionary.Remove(item.data);
        equippedItems.Remove(item);

        // 移除装备属性
        (item.data as ItemData_Equipment)?.RemoveModifiers();
    }


    private void UpdateSlotUI()
    {
        CleanUpSlotUI(); // 清理现有的UI槽位

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            itemSlot[i].UpdateSlot(inventoryItems[i]);
        }


        for (int i = 0; i < materialItems.Count; i++)
        {
            materialSlot[i].UpdateSlot(materialItems[i]);
        }


        //已装备武器的背包，根据装备类型更新到特定槽位
        for (int i = 0; i < equippedItems.Count; i++)
        {
            foreach (var slot in equippedSlot)
            {
                if (equippedItems[i].data is ItemData_Equipment equipmentData && slot.equipmentType == equipmentData.equipmentType)
                {
                    slot.UpdateSlot(equippedItems[i]);
                }
            }
        }
    }


    // 清理所有槽位的UI显示，在Remove时需要调用
    private void CleanUpSlotUI()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < materialSlot.Length; i++)
        {
            materialSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < equippedSlot.Length; i++)
        {
            equippedSlot[i].CleanUpSlot();
        }
    }


    // 不能用于已装备背包
    public void AddItem(ItemData _item)
    {
        // 如果是材料，添加到材料背包
        if (_item.itemType == ItemType.Material)
        {
            AddToMaterialInventory(_item);
        }

        // 如果是装备，添加到普通背包
        if (_item.itemType == ItemType.Equipment)
        {
            AddToInventory(_item);
        }

        UpdateSlotUI(); // 更新UI显示
    }

    // 从背包中移除物品，只能移除普通背包
    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                // 如果堆叠数量为 1，从字典和列表中移除物品
                inventoryDictionary.Remove(_item);
                inventoryItems.Remove(value);
            }
            else
            {
                // 如果堆叠数量大于 1，减少堆叠数量
                value.RemoveStack();
            }
        }

        CleanUpSlotUI();
        UpdateSlotUI(); // 更新UI显示

    }

    #endregion



    // 装备物品，UI_Slot中点击调用
    public void UnequipItem(InventoryItem item_to_equip)
    {

        // 卸下同类型的武器
        for (int i = equippedItems.Count - 1; i >= 0; i--)
        {
            var item = equippedItems[i];

            if ((item_to_equip.data as ItemData_Equipment).equipmentType == (item.data as ItemData_Equipment).equipmentType)
            {
                Unequip_Equipped_Item(item);
            }
        }


        // 装备到对应的槽位
        foreach (var slot in equippedSlot)
        {
            if (slot.equipmentType == (item_to_equip.data as ItemData_Equipment).equipmentType)  // 检查槽位类型是否匹配
            {
                //添加到已装备武器的列表
                InventoryItem newItem = new InventoryItem(item_to_equip.data);

                equippedItems.Add(newItem);
                equippedDictionary.Add(item_to_equip.data, newItem);

                // 从背包中移除已装备的物品
                RemoveItem(item_to_equip.data);

                // 添加装备属性到玩家
                (item_to_equip.data as ItemData_Equipment)?.AddModifiers();
            }
        }


        UpdateSlotUI(); // 更新UI显示
    }


    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        // 检查材料背包中是否有足够的材料
        foreach (var requiredMaterial in _requiredMaterials)
        {
            if (!materialDictionary.TryGetValue(requiredMaterial.data, out InventoryItem materialItem) ||
                materialItem.stackSize < requiredMaterial.stackSize)
            {
                return false; // 如果缺少任何材料，返回false
            }
        }
        return true; // 所有材料都满足要求
    }


    #region AddItem的内部逻辑
    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            // 如果物品已存在，增加堆叠数量
            value.AddStack();
        }
        else
        {
            // 如果物品不存在，创建新的InventoryItem，用字典反映数量
            InventoryItem newItem = new InventoryItem(_item);
            inventoryDictionary.Add(_item, newItem);
            inventoryItems.Add(newItem); // 添加到物品列表
        }
    }

    private void AddToMaterialInventory(ItemData _item)
    {
        if (materialDictionary.TryGetValue(_item, out InventoryItem value))
        {
            // 如果物品已存在，增加堆叠数量
            value.AddStack();
        }
        else
        {
            // 如果物品不存在，创建新的InventoryItem，用字典反映数量
            InventoryItem newItem = new InventoryItem(_item);
            materialDictionary.Add(_item, newItem);
            materialItems.Add(newItem); // 添加到物品列表
        }
    }

    #endregion

}

