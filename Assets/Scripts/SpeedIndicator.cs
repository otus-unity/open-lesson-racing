using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedIndicator : MonoBehaviour
{
    TextMeshProUGUI text;
    public Car car;
    int displayedSpeed = 0;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int speed = (int)car.speedKmh;
        if (displayedSpeed != speed) {
            text.text = $"{displayedSpeed} km/h";
            displayedSpeed = speed;
        }
    }
}
