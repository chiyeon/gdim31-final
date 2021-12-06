using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimCircles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * 8);
        transform.Rotate(transform.up * Time.deltaTime * 25, Space.World);
    }
}
