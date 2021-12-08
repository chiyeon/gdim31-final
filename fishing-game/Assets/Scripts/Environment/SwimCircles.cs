using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimCircles : MonoBehaviour
{
    [SerializeField] private Transform fish;
    [SerializeField] private GameObject Ripple;
    private GameObject rippleInstance;

    // Update is called once per frame
    void Update()
    {
        if(InventoryManager.instance.GetHasCursedBait() && InventoryManager.instance.GetHasCursedRod()) {
            fish.localPosition = Vector3.Lerp(fish.localPosition, Vector3.zero, Time.deltaTime);
            fish.localRotation = Quaternion.Lerp(fish.localRotation, Quaternion.Euler(new Vector3(-30, 0, 0)), Time.deltaTime);
            if(!rippleInstance) {
                fish.GetComponentInChildren<Rotate>().enabled = false;
                fish.GetChild(0).transform.localRotation = Quaternion.Euler(Vector3.zero);

                rippleInstance = Instantiate(Ripple, transform.position, Quaternion.identity);
            }
        } else {
            fish.Translate(fish.forward * Time.deltaTime * 8);
            fish.Rotate(fish.up * Time.deltaTime * 25, Space.World);
        }
    }
}
