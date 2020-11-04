using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public Path path;
    public Car car { get; private set; }
    public float sensorFrontOffset;
    public float sensorSideOffset;
    public float sensorEndShift;
    public float sensorLength;
    public LayerMask layerMask;
    public float distanceThreshold;
    public int nextWaypoint;
    public Player player;
    public bool isPolice;

    void Awake()
    {
        car = GetComponent<Car>();
        player = FindObjectOfType<Player>();
        FindClosestWaypoint();
    }

    void FindClosestWaypoint()
    {
        float closestDistance = float.MaxValue;
        int closestWaypoint = -1;

        Vector3 myPosition = transform.position;
        for (int i = 0; i < path.waypoints.Length; i++) {
            float distance = Vector3.Distance(path.waypoints[i].transform.position, myPosition);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestWaypoint = i;
            }
        }

        nextWaypoint = closestWaypoint;
    }

    Vector3 GetSensorStart(float dir)
    {
        return transform.position + transform.forward * sensorFrontOffset + transform.right * sensorSideOffset * dir;
    }

    Vector3 GetSensorDir(float shift)
    {
        return transform.forward * sensorLength + transform.right * shift * sensorEndShift;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(GetSensorStart( 0.0f), GetSensorDir( 0.0f)); // center
        Gizmos.DrawRay(GetSensorStart(-1.0f), GetSensorDir(-1.0f)); // left diagonal
        Gizmos.DrawRay(GetSensorStart(-1.0f), GetSensorDir( 0.0f)); // left direct
        Gizmos.DrawRay(GetSensorStart( 1.0f), GetSensorDir( 1.0f)); // right diagonal
        Gizmos.DrawRay(GetSensorStart( 1.0f), GetSensorDir( 0.0f)); // right direct
    }

    void Update()
    {
        Vector3 targetWaypoint = path.waypoints[nextWaypoint].transform.position;
        Vector3 target = new Vector3(targetWaypoint.x, transform.position.y, targetWaypoint.z);

        Vector3 vectorToTarget = transform.InverseTransformPoint(target);
        float distanceToTarget = vectorToTarget.magnitude;

        if (distanceToTarget <= distanceThreshold)
            nextWaypoint = (nextWaypoint + 1) % path.waypoints.Length;

        float forward = 1.0f;
        float steer = vectorToTarget.x / distanceToTarget;

        if (isPolice) {
            targetWaypoint = player.transform.position;
            target = new Vector3(targetWaypoint.x, transform.position.y, targetWaypoint.z);

            Vector3 vectorToPlayer = transform.InverseTransformPoint(target);
            float distanceToPlayer = vectorToPlayer.magnitude;

            if (distanceToPlayer < distanceToTarget)
                steer = vectorToPlayer.x / distanceToPlayer;
        }

        if (Physics.Raycast(GetSensorStart( 0.0f), GetSensorDir( 0.0f), sensorLength, layerMask)) // center
            forward = -1.0f;

        if (Physics.Raycast(GetSensorStart(-1.0f), GetSensorDir(-1.0f), sensorLength, layerMask)) // left diagonal
            steer = 1.0f;
        if (Physics.Raycast(GetSensorStart(-1.0f), GetSensorDir( 0.0f), sensorLength, layerMask)) // left direct
            steer = 1.0f;

        if (Physics.Raycast(GetSensorStart( 1.0f), GetSensorDir( 0.0f), sensorLength, layerMask)) // right diagonal
            steer = -1.0f;
        if (Physics.Raycast(GetSensorStart( 1.0f), GetSensorDir( 1.0f), sensorLength, layerMask)) // right direct
            steer = -1.0f;

        car.forward = forward;
        car.turn = steer;
    }
}
