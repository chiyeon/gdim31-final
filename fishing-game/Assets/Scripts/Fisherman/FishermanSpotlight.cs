using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishermanSpotlight : MonoBehaviour
{
    [SerializeField] private Fisherman fisherman;
    [SerializeField] private LayerMask layer;

    void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Bobber")) {
            Transform target = collider.transform;

            Vector3 dir = (target.position - transform.position).normalized;

            RaycastHit hit;
            Ray ray = new Ray(transform.position, dir);

            if(Physics.Raycast(ray, out hit, 1000, layer)) {
                if(hit.transform == target) {
                    fisherman.SetPlayer();
                } else {
                    Debug.Log("detected player but no clear line of site...");
                    Debug.Log("hit a " + hit.transform.gameObject.name);
                }
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if(collider.gameObject.CompareTag("Player")) {
            fisherman.ClearPlayer();
        }
    }

    
}
