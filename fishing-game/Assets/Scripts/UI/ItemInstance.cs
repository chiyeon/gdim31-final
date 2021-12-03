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

    public void Set(Item _item) {
        item = _item;
        icon.sprite = _item.GetIcon();
    }

    public static Item GetMouseOverItem() {
        return MouseOverItem;
    }

    public void OnPointerEnter(PointerEventData data) {
        MouseOverItem = item;
    }

    public void OnPointerExit(PointerEventData data) {
        MouseOverItem = null;
    }
}
