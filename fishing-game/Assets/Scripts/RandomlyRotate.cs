using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyRotate : MonoBehaviour
{
    [SerializeField] private float amount = 3.0f;
    private float tickRate = 0.5f;
    private float timer = 0;
    private Vector3 offset;
    private Vector3 targetRot;

    void Start() {
        offset = transform.localEulerAngles;
        tickRate = Random.Range(0.1f, 0.5f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > tickRate) {
            targetRot = new Vector3(Random.Range(-1f, 1f) * amount, Random.Range(-1f, 1f) * amount, Random.Range(-1f, 1f) * amount);
            timer = 0;
        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(offset + targetRot), Time.deltaTime * 10);
        
        //transform.Rotate(new Vector3(Random.Range(-1f, 1f) * Time.deltaTime * amount, Random.Range(-1f, 1f) * Time.deltaTime * amount, Random.Range(-1f, 1f) * Time.deltaTime * amount));
    }
}
