using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Cursed Rod")]
public class CursedRod : InteractableItem
{
    public override void Interact() {
        FishingController.instance.EquipCursedRod();
    }
}
