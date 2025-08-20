using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampTutorial : MonoBehaviour
{
    public GameObject player;
    public GameObject campTutorial;
    public CharacterController characterController;
    private BoxCollider col;


    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other = player.GetComponent<Collider>())
        {
            StartCoroutine(Tutorial());
        }
    }

    private IEnumerator Tutorial()
    {
        
        yield return new WaitForSeconds(1f);
        campTutorial.SetActive(true);
        characterController.enabled = false;
        //Time.timeScale = 0f;
        //take away player input 


        yield return new WaitForSeconds(5);
        //Time.timeScale = 1f;
        //give player input back
        characterController.enabled = true;
        campTutorial.SetActive(false);
        col.enabled = false;
        
    }
}
