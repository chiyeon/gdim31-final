using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlane : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject[] waters;
    [SerializeField] private GameObject[] models;
    private int currentModel = 0;
    private bool spawnNext = false;

    void Update() {
        foreach(GameObject water in waters) {
            water.transform.Translate(-transform.right * Time.deltaTime * moveSpeed);

            if(spawnNext && currentModel < models.Length) {
                spawnNext = false;
                Instantiate(models[currentModel], water.transform.GetChild(0));
                currentModel++;
            }

            if(water.transform.localPosition.x <= -125) {
                water.transform.localPosition = new Vector3(water.transform.localPosition.x+300, 0, 0);
                Destroy(water.transform.GetChild(0).transform.GetChild(0).gameObject);     // delete current model
                spawnNext = true;
            }
        }
    }
}
