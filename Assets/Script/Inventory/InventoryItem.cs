using System;

[Serializable]
public class InventoryItem
{
    //该脚本管理背包中的物品堆叠信息
    //一个Item对应一个InventoryItem

    public ItemData data;  //物品数据
    public int stackSize;  //堆叠数量


    //脚本初始化，获取物品信息
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        stackSize = 1;  //初始堆叠数量为0
    }


    //堆叠数量增加
    public void AddStack()
    {
        stackSize++;
    }

    public void RemoveStack()
    {
        stackSize--;
    }
}
