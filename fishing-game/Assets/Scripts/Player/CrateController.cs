using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{

    [Header("References")]
    private Trigger trigger;
    [SerializeField] private Transform ChestModel;
    [SerializeField] private Transform ChestPosition;
    // serialized reference to crate model

    void Start() {
        trigger = GetComponent<Trigger>();
    }

    void Update() {
        ChestModel.position = Vector3.Lerp(ChestModel.position, ChestPosition.position, Time.deltaTime * 2);
    }

    public void Open() {
        trigger.OnTrigger();
        Destroy(gameObject);
    }
}
