using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameWinUI : MonoBehaviour
{
    //canvas declarations
    private GameObject winCanvas; //to be enabled upon win
    //to be disabled upon win
    private GameObject playUI;
    private GameObject gameOverUI;
    private GameObject pauseUI;
    private GameObject towerStatsUI;

    private WaveManager waveManager;
    public bool test;
    // Start is called before the first frame update
    void Start()
    {
        waveManager = GameObject.Find("Spawner").GetComponent<WaveManager>();

        playUI = GameObject.Find("PlayUI");
        gameOverUI = GameObject.Find("GameOverUI");
        pauseUI = GameObject.Find("PauseUI");
        towerStatsUI = GameObject.Find("TowerStatsUI");

        winCanvas = GameObject.Find("GameWinUI");
        winCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        test = waveManager.gameWon;
        StartCoroutine(ShowWinScreenWithDelay());
    }

    private IEnumerator ShowWinScreenWithDelay()
    {

        if (waveManager.gameWon)
        {
            yield return new WaitForSeconds(0f);
            if (winCanvas != null)
                winCanvas.SetActive(true);

            if (playUI != null)
                playUI.SetActive(false);

            if (pauseUI != null)
                pauseUI.SetActive(false);

            if (gameOverUI != null)
                gameOverUI.SetActive(false);

            if (towerStatsUI != null)
                towerStatsUI.SetActive(false);
        }
    }
}
