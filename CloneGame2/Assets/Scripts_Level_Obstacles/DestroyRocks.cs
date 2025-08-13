using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRocks : MonoBehaviour
{
    public GameObject rockPrefab;
    void Start()
    {
        Destroy(rockPrefab, 5);
    }

}
