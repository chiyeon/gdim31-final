using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDelay : Trigger
{
    [SerializeField]
    private float delay = 0.0f;

    void Start() {
        StartCoroutine(LateStart());
    }
    
    IEnumerator LateStart() {
        //yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(delay);
        OnTrigger();
    }
}
