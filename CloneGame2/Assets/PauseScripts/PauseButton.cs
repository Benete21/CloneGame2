using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public GameObject pauseButton;
    public Button pause;

    public void Pause()
    {
        pause = GetComponent<Button>();
        pause.gameObject.SetActive(false);
        
        pauseButton.SetActive(true);
    }
    
       
    
}
