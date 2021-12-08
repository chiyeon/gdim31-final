using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private GameObject star;
    InventoryManager inv;
    // Start is called before the first frame update
    void Start()
    {
        inv = InventoryManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(inv.GetHasCursedRod()) {
            star.SetActive(true);
        }
    }
}
