using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private bool disableOnTrigger;

    [SerializeField] private List<Event> events;

    public void OnTrigger()
    {
        foreach(Event e in events) {
            e.OnEvent();
        }
    }
}
