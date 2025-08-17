using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour
{
    public void Return()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        

        if (currentSceneIndex - 1 == 0)
        {
            SceneManager.LoadScene(0);
        }

        else if (currentSceneIndex - 1 == 1)
        {
            SceneManager.LoadScene(1);

        }
                
                

    }
}
