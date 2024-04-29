using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    private PlayerStats PlayerStats;

    public GameObject playUI;
    public GameObject gameOverUI;
    public GameObject pauseUI;
    public GameObject towerStatsUI;

    // Start is called before the first frame update
    void Start()
    {
        HideGO();
        PlayerStats = GameObject.Find("PlaceholderManager").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.PlayerHP <= 0) //if the player has no hp, show game over screen
       {
            Time.timeScale = 0f;
            ShowGO();
        }
    }

    public void ShowGO() {
        gameOverUI.gameObject.SetActive(true);
        if (playUI != null)
            playUI.SetActive(false);

        if (pauseUI != null)
            pauseUI.SetActive(false);

        if (gameOverUI != null)
            towerStatsUI.SetActive(false);
    }

    public void HideGO()
    {
        gameOverUI.gameObject.SetActive(false);
    }

}
