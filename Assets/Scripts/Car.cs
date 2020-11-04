using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float acceleration;
    public float maxSteeringAngle;
    public Wheel[] frontWheels;
    public Wheel[] backWheels;

    [Range(-1, 1)]
    public float forward;
    [Range(-1, 1)]
    public float turn;

    public float speedKmh;
    new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        speedKmh = rigidbody.velocity.magnitude * 3.2f;
    }

    void FixedUpdate()
    {
        foreach (var wheel in frontWheels) {
            wheel.collider.motorTorque = forward * acceleration;
            wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, turn * maxSteeringAngle, 0.5f);
        }
    }
}
