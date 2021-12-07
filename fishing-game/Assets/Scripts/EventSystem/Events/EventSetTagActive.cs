using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSetTagActive : Event
{
    [SerializeField] private string target;
    [SerializeField] private bool activeSelf;

    public override void OnEvent() {
        GameObject.FindGameObjectWithTag(target).GetComponent<Trigger>().enabled = activeSelf;
    }
}
