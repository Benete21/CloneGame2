using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level_Obstacles : MonoBehaviour
{
    public GameObject rockPrefab; // Assign your rock Prefab
    public GameObject birdPrefab;   //Assign your bird Prefab
    public float spawnIntervalMinRock; // Minimum time between spawns for rocks
    public float spawnIntervalMaxRock; // Maximum time between spawns for rocks
    public float spawnIntervalMinBird;  // Minimum time between spawns for bird
    public float spawnIntervalMaxBird;  // Maximum time between spawns for bird
    public List<Vector3> SpawnPointsRock = new List<Vector3>();
    public List<Vector3> SpawnPointsBird = new List<Vector3>();

    public GameObject player;

    void Start()
    {
       /* StartCoroutine(SpawnRocksCoroutine());
        StartCoroutine(SpawnBirdCoroutine());*/
    }
    public void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;
        if (player.CompareTag ("Obstacle1"))
        {
            StartCoroutine(SpawnRocksCoroutine());
            StartCoroutine(SpawnBirdCoroutine());
        }
    }
    IEnumerator SpawnRocksCoroutine()
    {
        while (true)
        {
            // Randomize spawn time
            float randomTime = Random.Range(spawnIntervalMinRock, spawnIntervalMaxRock);
            yield return new WaitForSeconds(randomTime);

            // Randomize X position
            foreach (Vector3 i in SpawnPointsRock)
            {
                Instantiate(rockPrefab, i, Quaternion.identity);
            }
        }

    }
    IEnumerator SpawnBirdCoroutine()
    {
        while (true)
        {
            // Randomize spawn time
            float randomTime = Random.Range(spawnIntervalMinBird, spawnIntervalMaxBird);
            yield return new WaitForSeconds(randomTime);

            // Randomize X position
            foreach (Vector3 i in SpawnPointsBird)
            {
                Instantiate(birdPrefab, i, Quaternion.identity);
            }
        }
    }
}
