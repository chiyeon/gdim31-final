using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public void Awake() {
        instance = this;        // singleton
    }

    private IEnumerator IShake(float duration, float magnitude) {
        Vector3 startPos = transform.localPosition;     // save start
        float t = 0;
        while(t < duration) {           // for duration...
            // randomize pos
            transform.localPosition = new Vector3(Random.Range(-1f, 1f) * magnitude, Random.Range(-1f, 1f) * magnitude, startPos.z);
            t += Time.deltaTime;        // pass time
            yield return null;          // wait a frame
        }

        transform.localPosition = startPos;     // when done reset pos hehe
    }

    public void Shake(float duration, float magnitude) {
        StartCoroutine(IShake(duration, magnitude));
    }
}
