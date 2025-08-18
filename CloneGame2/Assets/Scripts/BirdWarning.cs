using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdWarning : MonoBehaviour
{
    public GameObject player;
    public GameObject BirdTutorial;

    private IEnumerator Tutorial()
    {

        yield return new WaitForSeconds(1f);
        BirdTutorial.SetActive(true);
        //Time.timeScale = 0f;
        //take away player input 


        yield return new WaitForSeconds(5);
        //Time.timeScale = 1f;
        //give player input back
        Destroy(BirdTutorial);
        Destroy(this.gameObject);

    }
}
