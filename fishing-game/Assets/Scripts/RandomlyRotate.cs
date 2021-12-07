using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyRotate : MonoBehaviour
{
    [SerializeField] private float amount = 3.0f;
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(Random.Range(-1f, 1f) * Time.deltaTime * amount, Random.Range(-1f, 1f) * Time.deltaTime * amount, Random.Range(-1f, 1f) * Time.deltaTime * amount));
    }
}
