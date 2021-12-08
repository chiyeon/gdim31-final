using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] private int zoneID;
    [SerializeField] private Waypoint startWaypoint;

    public int GetZoneID() {
        return zoneID;
    }

    public void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.CompareTag("Player")) {
            PlayerController.instance.SetZone(zoneID, gameObject);
            if(startWaypoint) {
                Fisherman.instance.TravelNewWaypoint(startWaypoint);
            }
        }
    }

    public void OnTriggerExit(Collider collider) {
        if(collider.gameObject.CompareTag("Player")) {
            PlayerController.instance.SetZone(0, null);
        }
    }
}
