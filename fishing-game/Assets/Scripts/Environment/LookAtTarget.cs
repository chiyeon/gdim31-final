using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// look at a Transform
public class LookAtTarget : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
            transform.LookAt(target);
    }
}
