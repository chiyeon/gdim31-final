using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public Vector3 boatPosition;
    public Quaternion boatRotation;

    public bool hasCursedRod;
    public bool hasCursedBait;
    public Item[] items;
    public Item[] pages;

    public List<bool> zones;

    public SaveData(
        Vector3 _boatPosition, Quaternion _boatRotation,
        bool _hasCursedRod, bool _hasCursedBait, 
        Item[] _items, Item[] _pages, List<bool> _zones) {

        boatPosition = _boatPosition;
        boatRotation = _boatRotation;
        
        hasCursedRod = _hasCursedRod;
        hasCursedBait = _hasCursedBait;
        items = _items;
        pages = _pages;
        zones = _zones;
    }
}
