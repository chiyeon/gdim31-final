using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : Item
{
    [SerializeField] protected AudioClip[] interactSounds;
    [SerializeField] private bool randomPitch = false;
    public virtual void Interact() {
        if(interactSounds.Length > 0) {
            if(randomPitch) {
                InventoryManager.instance.PlaySoundRandPitch(interactSounds[Random.Range(0, interactSounds.Length)]);
            } else {
                InventoryManager.instance.PlaySound(interactSounds[Random.Range(0, interactSounds.Length)]);
            }
        }
    }
}
