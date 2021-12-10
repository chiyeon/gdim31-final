using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.CompareTag("Terrain"))
            Destroy(gameObject);
    }
}
