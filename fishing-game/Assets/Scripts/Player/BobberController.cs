using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float pullSpeed = 3f;
    public bool pulling = false;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    
    public void Launch(float force) {
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    void Update() {
        if(pulling) {
            transform.position += transform.forward * pullSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider collider) {
        if((layerMask.value & (1 << collider.gameObject.layer)) > 0) {
            rb.isKinematic = true;
            transform.LookAt(FishingController.instance.transform);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            FishingController.instance.BobberLanded();
        } else if(collider.gameObject.CompareTag("FinishFishing")) {
            FishingController.instance.Catch();
        }
    }
}
