using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Cursed Component")]
public class CursedComponent : InteractableItem
{
    [SerializeField] private Item completedRod;
    [SerializeField] private Item[] components;
    public override void Interact() {
        for(int i = 0; i < components.Length; i++) {
            InventoryManager.instance.RemoveItem(components[i]);
        }

        InventoryManager.instance.AddItem(completedRod);
    }
}
