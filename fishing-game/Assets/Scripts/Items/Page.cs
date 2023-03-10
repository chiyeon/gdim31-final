using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Page")]
public class Page : InteractableItem
{
    [SerializeField][TextArea(3, 10)] private string note;
    [SerializeField] private bool isFinalPage;
    [SerializeField] private int pageNum = 1;

    public override void Interact() {
        base.Interact();
        UIInventory.instance.ShowNote(note);

        if(isFinalPage)
            UIInventory.instance.UpgradeCursedComponents();
         
         if (InventoryManager.instance.currentZone < pageNum) {
            UINotification.instance.ShowNotification("Map updated", 3);
            InventoryManager.instance.currentZone = pageNum;
         }
    }
}
