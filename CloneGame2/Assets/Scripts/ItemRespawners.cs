using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawners : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Items;
    [SerializeField]
    private GameObject[] Branches;

    private void Start()
    {
        Items = GameObject.FindGameObjectsWithTag("Fruit");
        Branches = GameObject.FindGameObjectsWithTag("Branch");
    }

    public void RespawnItems()
    {
        foreach (var item in Items)
        {
            item.gameObject.SetActive(true);
        }

        foreach (var item in Branches)
        {
            item.gameObject.SetActive(true);
        }
    }
}
