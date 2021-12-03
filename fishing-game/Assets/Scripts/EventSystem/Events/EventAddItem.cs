using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAddItem : Event
{
    [SerializeField] private Item item;

    public override void OnEvent() {
        InventoryManager.instance.AddItem(item);
    }
}
