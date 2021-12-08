using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Cursed Bait")]
public class CursedBait : InteractableItem
{
    public override void Interact() {
        InventoryManager.instance.SetHasCursedBait(true);
        InventoryManager.instance.RemoveItem(this);
    }
}
