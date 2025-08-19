using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using Random = UnityEngine.Random;


public class Level_Obstacles : MonoBehaviour
{
    public GameObject rockPrefab; // Assign your rock Prefab
    public GameObject birdPrefab;   //Assign your bird Prefab
    public float spawnIntervalMinRock; // Minimum time between spawns for rocks
    public float spawnIntervalMaxRock; // Maximum time between spawns for rocks
    public float spawnIntervalMinBird;  // Minimum time between spawns for bird
    public float spawnIntervalMaxBird;  // Maximum time between spawns for bird
    public List<GameObject> SpawnPointsRock = new List<GameObject>();
    public List<GameObject> SpawnPointsBird = new List<GameObject>();

    public GameObject player;
    public Collider playerCollider;
    public static event Action<Vector3, ObstacleType> OnObstacleSpawned;

    public enum ObstacleType { Rock, Bird }

    void Start()
    {
        playerCollider = player.GetComponent<Collider>();
        // StartCoroutine(SpawnRocksCoroutine());
         StartCoroutine(SpawnBirdCoroutine());

    }
    /*public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle1"))
        {
            StartCoroutine(SpawnBirdCoroutine());
        }
        else if (other.CompareTag("Player"))
        {
            StartCoroutine(SpawnRocksCoroutine());
            print("AHAHAHAHHAH");
        }
        else if (other.CompareTag("Obstacle3"))
        {

        }
    }*/

    public void StartBirdSpawn()
    {
        StartCoroutine(SpawnBirdCoroutine());
    }
    public void StartRockSpawn()
    {
        StartCoroutine(SpawnRocksCoroutine());
    }
    IEnumerator SpawnRocksCoroutine()
    {
        while (true)
        {
            // Randomize spawn time
            float randomTime = Random.Range(spawnIntervalMinRock, spawnIntervalMaxRock);
            yield return new WaitForSeconds(randomTime);

            // Randomize X position
            foreach (GameObject i in SpawnPointsRock)
            {
                Vector3 j = i.transform.position;
                Instantiate(rockPrefab, j, Quaternion.identity);

                OnObstacleSpawned?.Invoke(j, ObstacleType.Rock); //Notify indicators system
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
            foreach (GameObject i in SpawnPointsBird)
            {
                Vector3 j = i.transform.position;
                Instantiate(birdPrefab, j, Quaternion.identity);
                OnObstacleSpawned?.Invoke(j, ObstacleType.Bird); //notify indicator system
            }
        }
    }
    IEnumerator SpawnRocksRocksCoroutine()
    {
        while (true)
        {
            // Randomize spawn time
            float randomTime = Random.Range(spawnIntervalMinRock, spawnIntervalMaxRock);
            yield return new WaitForSeconds(randomTime);

            // Randomize X position
            foreach (GameObject i in SpawnPointsRock)
            {
                Vector3 j = i.transform.position;
                Instantiate(rockPrefab, j, Quaternion.identity);

                OnObstacleSpawned?.Invoke(j, ObstacleType.Rock); //Notify indicators system
            }

        }
    }
}
   
