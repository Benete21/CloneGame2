
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingFog : MonoBehaviour
{
    public float riseSpeed = 2f; // Speed at which the fog rises
    public List<float> stopHeights = new List<float> { 17f, 103f, 236f, 332f, 458f };
    private int currentTargetIndex = 0;
    private bool isRising = true;

    private float epsilon = 0.1f; // Tolerance for reaching target height

    void Update()
    {
        if (!isRising || currentTargetIndex >= stopHeights.Count)
            return;

        float targetHeight = stopHeights[currentTargetIndex];
        float step = riseSpeed * Time.deltaTime;

        // Move toward the target height
        if (transform.position.y < targetHeight)
        {
            transform.position += Vector3.up * step;

            // Clamp to exact target if we overshoot
            if (transform.position.y >= targetHeight - epsilon)
            {
                transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
                isRising = false; // Stop rising until re-enabled
                Debug.Log("Reached checkpoint at height: " + targetHeight);
            }
        }
    }

    // Call this method from another script or trigger to continue rising
    public void ContinueToNextHeight()
    {
        if (currentTargetIndex < stopHeights.Count - 1)
        {
            currentTargetIndex++;
            isRising = true;
        }
        else
        {
            Debug.Log("Reached final height.");
        }
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingFog : MonoBehaviour
{
    public float riseSpeed = 0.1f; // Speed at which the water rises
    public float maxWaterHeight = 10000f; // Maximum height the water will reach

    void Update()
    {
        // Move the water upwards
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        if (transform.position.y == 17f)
        {
            enabled = false;
        }
        else if (transform.position.y == 103f)
        {
            enabled = false;
        }
        else if (transform.position.y == 236f)
        {
            enabled = false;
        }
        else if (transform.position.y == 332f)
        {
            enabled = false;
        }
        else if (transform.position.y == 458f)
        {
            enabled = false;
        }
        if (transform.position.y >= maxWaterHeight)
        {
            enabled = false; 
        }
    }
}*/
