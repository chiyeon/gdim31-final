using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSetActive : Event
{
    [SerializeField] private GameObject target;
    [SerializeField] private bool activeSelf;

    public override void OnEvent() {
        target.SetActive(activeSelf);
    }
}
