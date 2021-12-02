using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// follows a target Transform, with an offset based on the initial distances between the objects
public class Follow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;

    void Start() {
        if(target)
            offset = transform.position - target.position;
    }
    void Update()
    {
        transform.position = target.position + offset;
    }
}
