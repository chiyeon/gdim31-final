using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint[] nextWaypoints;

    public Waypoint GetNextWaypoint() {
        return nextWaypoints[Random.Range(0, nextWaypoints.Length)];
    }
}
