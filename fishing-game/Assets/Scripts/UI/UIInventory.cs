using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public static UIInventory instance;

    [Header("References")]
    [SerializeField] private Transform itemsParent;
    [SerializeField] private Transform pagesParent;
    [SerializeField] private GameObject ItemInstanceObject;
    [SerializeField] private GameObject Tooltip;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject BookPanel;
    [SerializeField] private GameObject Note;
    [SerializeField] private AudioClip[] OpenInventorySounds;

    void Awake() {
        instance = this;
    }
    

    void Start() {
        InventoryPanel.SetActive(false);            // off by default
        Tooltip.SetActive(false);
        BookPanel.SetActive(false);
        Note.SetActive(false);
    }

    void Update() {
        if(Input.GetButtonDown("Inventory") && !PlayerController.instance.GetDisableControls()) OpenInventory();
        if(Input.GetButtonUp("Inventory")) CloseInventory();

        if(ItemInstance.GetMouseOverItem() != null) {
            Tooltip.SetActive(true);
            Tooltip.GetComponent<Tooltip>().Set(ItemInstance.GetMouseOverItem());
            Tooltip.transform.position = Input.mousePosition;
        } else {
            Tooltip.SetActive(false);
        }
    }

    public void OpenInventory() {
        PlayerController.instance.PlaySoundRandPitch(OpenInventorySounds[Random.Range(0, OpenInventorySounds.Length)]);
        InventoryPanel.SetActive(true);           // open panel
        Global.instance.SetFPSMouse(false);        // remove mouse lock
        PlayerController.instance.SetDisableControls(true);      // freeze player
        ItemInstance.SetMouseOverItem(null);
        BookPanel.SetActive(false);
        HideNote();
    }

    public void CloseInventory() {
        InventoryPanel.SetActive(false);           // open panel
        Global.instance.SetFPSMouse(true);        // remove mouse lock
        PlayerController.instance.SetDisableControls(false);      // freeze player
        ItemInstance.SetMouseOverItem(null);
    }

    public void AddItem(Item item, bool IsInteractable) {
        ItemInstance itemInstance = Instantiate(ItemInstanceObject, itemsParent).GetComponent<ItemInstance>();
        itemInstance.Set(item, IsInteractable);
    }

    public void AddPage(Item page) {
        ItemInstance itemInstance = Instantiate(ItemInstanceObject, pagesParent).GetComponent<ItemInstance>();
        itemInstance.Set(page, true);
    }

    public void ToggleBookUI() {
        BookPanel.SetActive(!BookPanel.activeSelf);
    }

    public bool BookPanelActive() {
        return BookPanel.activeSelf;
    }

    public void ShowNote(string _note) {
        Note.SetActive(true);
        Note.GetComponentInChildren<Text>().text = _note;
    }

    public void HideNote() {
        Note.SetActive(false);
    }

    public void RemoveItem(Item item) {
        foreach(Transform instance in itemsParent) {
            if(instance.GetComponent<ItemInstance>().GetItem() == item) {
                Destroy(instance.gameObject);
            }
        }
    }

    public void UpgradeCursedComponents() {
        foreach(Transform instance in itemsParent) {
            ItemInstance itemInstance = instance.GetComponent<ItemInstance>();
            if(itemInstance.GetItem() is CursedComponent) {
                itemInstance.Set(itemInstance.GetItem(), true);
            }
        }
    }
}
