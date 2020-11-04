using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Car car { get; private set; }

    void Awake()
    {
        car = GetComponent<Car>();
    }

    void Update()
    {
        car.forward = Input.GetAxis("Vertical");
        car.turn = Input.GetAxis("Horizontal");
    }
}
