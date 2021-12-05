using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAddPage : Event
{
    [SerializeField] private Item page;

    public override void OnEvent() {
        InventoryManager.instance.AddPage(page);
    }
}
