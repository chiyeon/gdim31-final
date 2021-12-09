using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Book")]
public class Book : InteractableItem
{
    [SerializeField] private AudioClip[] ClosingSounds;
    public override void Interact() {
        UIInventory.instance.ToggleBookUI();
        if(UIInventory.instance.BookPanelActive()) {
            InventoryManager.instance.PlaySoundRandPitch(interactSounds[Random.Range(0, interactSounds.Length)]);
        } else {
            InventoryManager.instance.PlaySoundRandPitch(ClosingSounds[Random.Range(0, ClosingSounds.Length)]);
        }
    }
}
