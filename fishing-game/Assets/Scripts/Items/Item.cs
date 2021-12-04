using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    [SerializeField] private new string name;
    [TextArea(2, 3)][SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private bool interactable;

    public string GetName() {
        return name;
    }

    public string GetDescription() {
        return description;
    }

    public Sprite GetIcon() {
        return icon;
    }

    public bool IsInteractable() {
        return interactable;
    }
}
