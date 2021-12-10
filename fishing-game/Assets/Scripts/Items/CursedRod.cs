using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Cursed Rod")]
public class CursedRod : InteractableItem
{
    public override void Interact() {
        base.Interact();
        UINotification.instance.ShowNotification("You equip a Cursed Rod", 3);
        InventoryManager.instance.SetHasCursedRod(true);
        InventoryManager.instance.RemoveItem(this);
            Global.instance.SaveGame();
    }
}
