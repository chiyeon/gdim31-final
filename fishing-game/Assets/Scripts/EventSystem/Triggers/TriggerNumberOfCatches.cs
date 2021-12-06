using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNumberOfCatches : Trigger
{
    private int catches = 0;
    [SerializeField] private int numCatches = 0;

    public void IncreaseCatch() {
        catches++;
        if(catches >= numCatches) {
            OnTrigger();
        }
    }
}
