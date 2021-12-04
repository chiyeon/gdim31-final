using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Book")]
public class Book : InteractableItem
{
    public override void Interact() {
        UIInventory.instance.ToggleBookUI();
    }
}
