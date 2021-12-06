using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollision : Trigger
{
    [SerializeField] private string searchTag = "";

    void OnTriggerEnter(Collider collider) {
        if(searchTag != "") {
            if(!collider.gameObject.CompareTag(searchTag)) {
                return;
            }
        }

        OnTrigger();
    }
}
