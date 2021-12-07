using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// look at a Transform
public class LookAtTarget : MonoBehaviour
{
    public Transform target;
    public string targetTag;
    // Start is called before the first frame update
    void Start()
    {
        if(targetTag != null) {
            if(targetTag != "") {
                target = GameObject.FindGameObjectWithTag(targetTag).transform;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
            transform.LookAt(target);
    }
}
