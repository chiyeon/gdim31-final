using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    [SerializeField] private float bobRate;
    [SerializeField] private float bobDistance;
    
    void Update() {
        transform.localPosition = new Vector3(0, Mathf.Sin(Time.time * bobRate) * bobDistance, 0);
    }
}
