using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampTutorial : MonoBehaviour
{
    public GameObject player;
    public GameObject campTutorial;
    
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
        //Time.timeScale = 0f;
        //take away player input 


        yield return new WaitForSeconds(5);
        //Time.timeScale = 1f;
        //give player input back
        Destroy(campTutorial);
        Destroy(this.gameObject);
        
    }
}
