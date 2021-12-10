using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOnStart : MonoBehaviour
{
    void Start() {
        StartCoroutine(Load());
    }

    IEnumerator Load() {
        yield return new WaitForEndOfFrame();
        Global.instance.LoadGame();
    }
}
