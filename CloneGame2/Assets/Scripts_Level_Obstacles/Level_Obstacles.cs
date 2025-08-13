using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level_Obstacles : MonoBehaviour
{
    public GameObject rockPrefab; // Assign your rock Prefab in the Inspector
    public float spawnIntervalMin; // Minimum time between spawns
    public float spawnIntervalMax; // Maximum time between spawns
    public List<Vector3> SpawnPoints = new List<Vector3>();


    void Start()
    {
        StartCoroutine(SpawnRocksCoroutine());
    }

    IEnumerator SpawnRocksCoroutine()
    {
        while (true)
        {
            // Randomize spawn time
            float randomTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(randomTime);

            // Randomize X position
            foreach (Vector3 i in SpawnPoints)
            {
                Instantiate(rockPrefab, i, Quaternion.identity);
            }
        }

    }
}
