using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTut : MonoBehaviour
{
    public GameObject player;
    public CharacterController characterController;
    public GameObject ObstacleTutorial;

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
        ObstacleTutorial.SetActive(true);
        characterController.enabled = false;
        //Time.timeScale = 0f;
        //take away player input 


        yield return new WaitForSeconds(5);
        //Time.timeScale = 1f;
        //give player input back
        characterController.enabled = true; 
        Destroy(ObstacleTutorial);
        Destroy(this.gameObject);

    }
}
