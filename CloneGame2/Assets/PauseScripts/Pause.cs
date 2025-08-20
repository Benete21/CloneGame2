using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public Button pause;
    public GameObject pauseMenu;
    public GameObject howToPlay;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseB();
        }
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync(0);
        
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
    }

    public void PauseB()
    {
        
        pause.gameObject.SetActive(false);

        pauseMenu.SetActive(true);

        Time.timeScale = 0f;   
    }

   


}
