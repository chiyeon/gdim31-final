using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Inventory")]
    [SerializeField] private List<Item> items;
    [SerializeField] private List<Item> pages;
    private int numComponents = 0;

    void Awake() {
        instance = this;
    }

    public void AddItem(Item item) {
        items.Add(item);
        if(item is CursedComponent) {
            numComponents++;

            if(numComponents >= 3) {
                UIInventory.instance.AddItem(item, true);
                UIInventory.instance.UpgradeCursedComponents();
            } else {
                UIInventory.instance.AddItem(item, false);
            }
            return;
        }
        UIInventory.instance.AddItem(item, item.IsInteractable());
    }

    public void AddPage(Item page) {
        pages.Add(page);
        UIInventory.instance.AddPage(page);
    }

    public void RemoveItem(Item item) {
        if(items.Contains(item)) {
            items.Remove(item);
            UIInventory.instance.RemoveItem(item);
        }
    }
 
}
