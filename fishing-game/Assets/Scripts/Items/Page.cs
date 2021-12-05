using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Page")]
public class Page : InteractableItem
{
    [SerializeField][TextArea(3, 10)] private string note;

    public override void Interact() {
        UIInventory.instance.ShowNote(note);
    }
}
