using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public Button pause;
    public GameObject pauseMenu;
    //public Canvas healthbars;
 
    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync(0);   
    } 

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseB(); // Call your function
        }

       
    }

    public void Return()
    {
        
        pauseMenu.SetActive(true);
        pause.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
    

    public void Play()
    {
        pause.gameObject.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
       // healthbars.gameObject.SetActive(true);
    }

    public void PauseB()
    {
        
        pause.gameObject.SetActive(false);

        pauseMenu.SetActive(true);

        Time.timeScale = 0f;   

        //healthbars.gameObject.SetActive(false);
    }

   


}
