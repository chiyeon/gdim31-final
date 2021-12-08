using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemInstance : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static Item MouseOverItem;

    private Item item;

    [SerializeField] private Image icon;
    [SerializeField] private Image interactableHint;

    public void Set(Item _item, bool IsInteractable) {
        item = _item;
        icon.sprite = _item.GetIcon();
        if(IsInteractable) {
            interactableHint.gameObject.SetActive(true);
            GetComponent<Button>().enabled = true;
        } else {
            interactableHint.gameObject.SetActive(false);
            GetComponent<Button>().enabled = false;
        }
    }

    public static Item GetMouseOverItem() {
        return MouseOverItem;
    }

    public static void SetMouseOverItem(Item item) {
        MouseOverItem = item;
    }

    public void OnPointerEnter(PointerEventData data) {
        MouseOverItem = item;
    }

    public void OnPointerExit(PointerEventData data) {
        MouseOverItem = null;
    }

    public void Interact() {
        if(item is InteractableItem) {
            ((InteractableItem)item).Interact();
        }
    }

    public Item GetItem() {
        return item;
    }
}
