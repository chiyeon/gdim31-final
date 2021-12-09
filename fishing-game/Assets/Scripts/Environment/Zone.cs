using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    private static int prevZoneID = -1;
    [SerializeField] private int zoneID;
    [SerializeField] private Waypoint startWaypoint;

    public int GetZoneID() {
        return zoneID;
    }

    public void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.CompareTag("Player")) {
            PlayerController.instance.SetZone(zoneID, gameObject);
            
            if(startWaypoint && prevZoneID != zoneID) {
                FishingController.instance.ResetCatchCounter();
                Fisherman.instance.TravelNewWaypoint(startWaypoint);
                prevZoneID = zoneID;
            }
        }
    }

    public void OnTriggerExit(Collider collider) {
        if(collider.gameObject.CompareTag("Player")) {
            PlayerController.instance.SetZone(0, null);
        }
    }

    public Waypoint GetClosestWaypointToPlayer() {
        Waypoint closest = null;
        float closestDistance = 100000;
        foreach(Transform waypoint in transform) {
            float dist = Vector3.Distance(waypoint.position, transform.position);
            if(dist < closestDistance) {
                closestDistance = dist;
                closest = waypoint.GetComponent<Waypoint>();
            }
        }
        return closest;
    }
}
