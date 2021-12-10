using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Cursed Bait")]
public class CursedBait : InteractableItem
{
    public override void Interact() {
        if(InventoryManager.instance.GetHasCursedRod()) {
            base.Interact();
            InventoryManager.instance.SetHasCursedBait(true);
            InventoryManager.instance.RemoveItem(this);
            Global.instance.SaveGame();
        } else {
            UINotification.instance.ShowNotification("It doesn't fit on your normal fishing rod.", 3);
        }
    }
}
