using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{

    [Header("References")]
    private Trigger trigger;
    // serialized reference to crate model

    void Start() {
        trigger = GetComponent<Trigger>();
    }

    void Update() {
        // make crate bob up and down
    }

    public void Open() {
        trigger.OnTrigger();
        Destroy(gameObject);
    }
}
