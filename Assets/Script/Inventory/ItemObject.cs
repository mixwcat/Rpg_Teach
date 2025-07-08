using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemData itemData;


    //编辑器中即可改变物品信息
    void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon; // 在编辑器中设置物品图标
        gameObject.name = itemData.itemName; // 设置物品名称
    }
    

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = itemData.icon; // 设置物品图标
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 触发与玩家的碰撞
            Inventory.instance.AddItem(itemData); // 将物品添加到背包
            Destroy(gameObject);
        }
    }
}
