using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Inventory")]
    [SerializeField] private List<Item> items;
    [SerializeField] private List<Item> pages;
    [SerializeField] private AudioSource InventoryAudioSource;
    //private int numComponents = 0;
    private bool hasCursedRod = false;
    private bool hasCursedBait = false;
    public int currentZone = 0;

    void Awake() {
        instance = this;
    }

    public void PlaySound(AudioClip clip) {
        InventoryAudioSource.pitch = 1;
        InventoryAudioSource.PlayOneShot(clip);
    }

    public void PlaySoundRandPitch(AudioClip clip) {
        InventoryAudioSource.pitch = Random.Range(0.75f, 1.25f);
        InventoryAudioSource.PlayOneShot(clip);
    }

    public void AddItem(Item item, bool notify = true) {
        if(notify)
            UINotification.instance.ShowNotification("You found a " + item.GetName(), 3);
        items.Add(item);
        if(item is CursedComponent) {
            UIInventory.instance.AddItem(item, false);
            return;
            /*numComponents++;
            
            if(numComponents >= 3) {
                UIInventory.instance.AddItem(item, true);
                UIInventory.instance.UpgradeCursedComponents();
            } else {
                UIInventory.instance.AddItem(item, false);
            }
            return;*/
        }
        UIInventory.instance.AddItem(item, item.IsInteractable());
    }

    public void AddPage(Item page, bool notify = true) {
        if(notify) {
            UINotification.instance.ShowNotification("You found " + page.GetName(), 3);
            //UINotification.instance.ShowNotification("Map updated", 3);
        }
        pages.Add(page);
        UIInventory.instance.AddPage(page);
        //currentZone++;
    }

    public void RemoveItem(Item item) {
        if(items.Contains(item)) {
            items.Remove(item);
            UIInventory.instance.RemoveItem(item);
        }
    }

    public bool GetHasCursedRod() {
        return hasCursedRod;
    }

    public bool GetHasCursedBait() {
        return hasCursedBait;
    }

    public void SetHasCursedBait(bool _hasCursedBait) {
        hasCursedBait = _hasCursedBait;
    }

    public void SetHasCursedRod(bool _hasCursedRod) {
        hasCursedRod = _hasCursedRod;

        if(hasCursedRod) {
            FishingController.instance.EquipCursedRod();
            currentZone++;
        }
   
    }

    /*
    public int GetNumComponents() {
        return numComponents;
    }*/

    public List<Item> GetItems() {
        return items;
    }

    public List<Item> GetPages() {
        return pages;
    }

    public void SetItems(Item[] items) {
        foreach(Item item in items) {
            AddItem(item, false);
        }
    }

    public void SetPages(Item[] pages) {
        foreach(Item item in pages) {
            AddPage(item, false);
        }
    }
 
}
