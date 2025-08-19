using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birds : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of movement
    public float moveDistance = 5f; // How far up and down it moves
    private Vector3 startPosition;
    public GameObject birdPrefab;

    void Start()
    {
        startPosition = transform.position; // Store the initial position
        Destroy(birdPrefab, 2);
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave for smooth oscillation
        float newX = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
