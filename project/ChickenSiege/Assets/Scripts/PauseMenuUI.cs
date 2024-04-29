using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public Button pauseButton;
    public bool isPaused = false;

    public Canvas playUI;
    public Canvas pauseUI;
    public Canvas towerStatsUI;
    public Canvas settingsUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPauseMenu()
    {
        pauseUI.gameObject.SetActive(true);

        //set time to zero
        Time.timeScale = 0f;

        //hide other game elements?
        playUI.gameObject.SetActive(false);
        towerStatsUI.gameObject.SetActive(false);
        settingsUI.gameObject.SetActive(false);



    }

    public void HidePauseMenu()
    {

        pauseUI.gameObject.SetActive(false);

        //set time back to 1
        Time.timeScale = 1f;

        //show playui
        playUI.gameObject.SetActive(true);
    }

    public void ShowSettingsMenu()
    {
        pauseUI.gameObject.SetActive(false);
        settingsUI.gameObject.SetActive(true);
    }
}
