using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemDescription;

    public void Set(Item item) {
        itemName.text = item.GetName();
        itemDescription.text = item.GetDescription();
    }
}
