using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInput : Trigger
{
    [SerializeField] private string input;

    void Update()
    {
        if(Input.GetButtonDown(input)) {
            OnTrigger();
        }
    }
}
