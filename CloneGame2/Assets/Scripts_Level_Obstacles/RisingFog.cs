using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingFog : MonoBehaviour
{
    public float riseSpeed = 0.1f; // Speed at which the water rises
    public float maxWaterHeight = 10f; // Maximum height the water will reach

    void Update()
    {
        // Move the water upwards
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        // Optional: Stop rising at a certain height
        if (transform.position.y >= maxWaterHeight)
        {
            // You can add logic here to stop rising, or start lowering, etc.
            enabled = false; // Disable the script
        }
    }
}
