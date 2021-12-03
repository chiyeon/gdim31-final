using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public static UIInventory instance;

    [Header("References")]
    [SerializeField] private Transform itemsParent;
    [SerializeField] private GameObject ItemInstanceObject;
    [SerializeField] private GameObject Tooltip;
    [SerializeField] private GameObject InventoryPanel;

    void Awake() {
        instance = this;
    }
    

    void Start() {
        InventoryPanel.SetActive(false);            // off by default
        Tooltip.SetActive(false);
    }

    void Update() {
        // open inventory while holding down alt
        if(Input.GetButtonDown("Inventory") || Input.GetButtonUp("Inventory")) {
            InventoryPanel.SetActive(!InventoryPanel.activeSelf);           // open panel
            Global.instance.SetFPSMouse(!InventoryPanel.activeSelf);        // remove mouse lock
            PlayerController.instance.SetDisableControls(!PlayerController.instance.GetDisableControls());      // freeze player
        }

        if(ItemInstance.GetMouseOverItem() != null) {
            Tooltip.SetActive(true);
            Tooltip.GetComponent<Tooltip>().Set(ItemInstance.GetMouseOverItem());
            Tooltip.transform.position = Input.mousePosition;
        } else {
            Tooltip.SetActive(false);
        }
    }

    public void AddItem(Item item) {
        ItemInstance itemInstance = Instantiate(ItemInstanceObject, itemsParent).GetComponent<ItemInstance>();
        itemInstance.Set(item);
    }
}
