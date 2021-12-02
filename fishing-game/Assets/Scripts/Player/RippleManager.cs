using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleManager : MonoBehaviour
{
    [SerializeField]
    private int maxRipples;
    [SerializeField]
    private float maxRadius;
    [SerializeField]
    private float waterLevel;
    private List<GameObject> ripples = new List<GameObject>();
    [SerializeField]
    private GameObject Ripple;

    void FixedUpdate() {
        if(ripples.Count < maxRipples) {
            // places them in a square. some spawn out of range but are quickly removed by the lower code!
            Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-maxRadius, maxRadius), waterLevel, transform.position.z + Random.Range(-maxRadius, maxRadius));
            GameObject newRipple = Instantiate(Ripple, spawnPosition, Quaternion.identity);
            ripples.Add(newRipple);
        }

        for(int i = 0; i < ripples.Count; i++) {
            if(ripples[i] == null) {
                // our ripple was deleted as part of bobber probably hitting it, just remove it and keep goin
                ripples.RemoveAt(i);
                continue;
            } else {
                if(Vector3.Distance(ripples[i].transform.position, transform.position) > maxRadius) {
                    Destroy(ripples[i]);
                    ripples.RemoveAt(i);
                }
            }
        }
    }
}
