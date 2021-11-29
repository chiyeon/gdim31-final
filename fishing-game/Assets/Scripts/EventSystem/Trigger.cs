using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private bool disableOnTrigger;

    public delegate void OnEventTrigger();
    public OnEventTrigger onEventTrigger;

    public void OnTrigger()
    {
        if (onEventTrigger != null)
            onEventTrigger();

        if (disableOnTrigger)
            onEventTrigger = null;
    }
}
