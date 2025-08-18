using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawners : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Items;
    [SerializeField]
    private GameObject[] Branches;
    [SerializeField]
    private GameObject[] Urchins;

    private void Start()
    {
        Items = GameObject.FindGameObjectsWithTag("Fruit");
        Branches = GameObject.FindGameObjectsWithTag("Branch");
        Urchins = GameObject.FindGameObjectsWithTag("Urchin");
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
        foreach(var item in Urchins)
        {
            item.gameObject.SetActive(true);
        }
    }
}
