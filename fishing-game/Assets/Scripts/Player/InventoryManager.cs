using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Inventory")]
    [SerializeField] private List<Item> items;
    [SerializeField] private List<Item> pages;

    void Awake() {
        instance = this;
    }

    public void AddItem(Item item) {
        items.Add(item);
        UIInventory.instance.AddItem(item);
    }

    public void AddPage(Item page) {
        pages.Add(page);
        UIInventory.instance.AddPage(page);
    }
 
}
