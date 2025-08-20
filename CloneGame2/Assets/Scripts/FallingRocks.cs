using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    [Header("Rock Settings")]
    public GameObject rockPrefab;
    public float spawnIntervalMinRock;
    public float spawnIntervalMaxRock;
    public List<GameObject> SpawnPointsRock = new List<GameObject>();
    public List<GameObject> SpawnPointsRockTop = new List<GameObject>();

    private Coroutine rockCoroutine;
    private Coroutine rockTopCoroutine;

    void OnEnable()
    {
        // Start rock spawning coroutines when GameObject becomes active
        rockCoroutine = StartCoroutine(SpawnRocksCoroutine());
        rockTopCoroutine = StartCoroutine(SpawnTopRocksCoroutine());
    }

    void OnDisable()
    {
        // Stop all coroutines when GameObject becomes inactive
        if (rockCoroutine != null)
        {
            StopCoroutine(rockCoroutine);
            rockCoroutine = null;
        }
        if (rockTopCoroutine != null)
        {
            StopCoroutine(rockTopCoroutine);
            rockTopCoroutine = null;
        }
    }

    IEnumerator SpawnRocksCoroutine()
    {
        while (true)
        {
            float randomTime = Random.Range(spawnIntervalMinRock, spawnIntervalMaxRock);
            yield return new WaitForSeconds(randomTime);

            foreach (GameObject spawnPoint in SpawnPointsRock)
            {
                Vector3 spawnPosition = spawnPoint.transform.position;
                Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    IEnumerator SpawnTopRocksCoroutine()
    {
        while (true)
        {
            float randomTime = Random.Range(spawnIntervalMinRock, spawnIntervalMaxRock);
            yield return new WaitForSeconds(randomTime);

            foreach (GameObject spawnPoint in SpawnPointsRockTop)
            {
                Vector3 spawnPosition = spawnPoint.transform.position;
                Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    // Optional: Public methods if you need manual control
    public void StartRockSpawning()
    {
        if (rockCoroutine == null)
            rockCoroutine = StartCoroutine(SpawnRocksCoroutine());
        if (rockTopCoroutine == null)
            rockTopCoroutine = StartCoroutine(SpawnTopRocksCoroutine());
    }

    public void StopRockSpawning()
    {
        if (rockCoroutine != null)
        {
            StopCoroutine(rockCoroutine);
            rockCoroutine = null;
        }
        if (rockTopCoroutine != null)
        {
            StopCoroutine(rockTopCoroutine);
            rockTopCoroutine = null;
        }
    }
}
