using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private Transform CheckPoint;
    private CharacterController controller;
    private StatManager statManagerScript;
    [SerializeField]
    private int DeathCount;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        statManagerScript = GetComponent<StatManager>();
    }

    private void Update()
    {
        if (statManagerScript.Hp <= 0)
        {
            if (DeathCount < 3)
            {
                Respawn();
            }
            else
            {
                GameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            CheckPoint = other.transform;
        }
    }



    void Respawn ()
    {
        if (CheckPoint != null)
        {
            controller.enabled = false;
            transform.position = CheckPoint.position;
            controller.enabled = true;
            statManagerScript.Hp = statManagerScript.MaxHp;
            DeathCount++;
        }
        else if (CheckPoint == null)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GameOver");
    }
}
