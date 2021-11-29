using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{
    [SerializeField]
    private Trigger trigger;

    private void Start() {
        if(trigger != null)
            trigger.onEventTrigger += OnEvent;
    }

    public abstract void OnEvent();
}
