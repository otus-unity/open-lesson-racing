using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Waypoint[] waypoints;

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2)
            return;

        Gizmos.color = Color.white;
        for (int i = 0; i < waypoints.Length; i++)
            Gizmos.DrawLine(waypoints[i].transform.position, waypoints[(i + 1) % waypoints.Length].transform.position);
    }
}
