using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{

    public GameObject EndGameImage;
    private void OnTriggerEnter(Collider other)
    {
       

        if (other.CompareTag("Player"))
        {
            EndGameImage.SetActive(true);
            Time.timeScale = 0f;  
        }
    }
}
