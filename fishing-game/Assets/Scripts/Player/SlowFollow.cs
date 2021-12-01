using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 0.5f;

    void Update() {
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * followSpeed);
    }
}
