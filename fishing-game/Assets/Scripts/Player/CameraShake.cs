using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public void Awake() {
        instance = this;
    }

    private IEnumerator IShake(float duration, float magnitude) {
        Vector3 startPos = transform.localPosition;
        float t = 0;
        while(t < duration) {
            transform.localPosition = new Vector3(Random.Range(-1f, 1f) * magnitude, Random.Range(-1f, 1f) * magnitude, startPos.z);

            t += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = startPos;
    }

    public void Shake(float duration, float magnitude) {
        StartCoroutine(IShake(duration, magnitude));
    }
}
