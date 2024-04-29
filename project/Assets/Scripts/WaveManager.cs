using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct Enemy
{
    public GameObject enemyPrefab;
    public int enemyCount;
}

[System.Serializable]
public struct Wave
{
    public float spawnInterval;
    public Enemy[] enemy;
}

public class WaveManager : MonoBehaviour
{
    public Transform spawnPoint; //enemysopawn
    public Wave[] waves; //array for wave info to adjust in inspector

    public int currentWaveIndex = 0;

    //two booleans used to keep the player from spamming "start wave"
    public bool waveInProgress = false; //boolean to determine if a wave is in progress or not
    public bool enemiesRemaining = false;//boolean to determine if any enemies are remaining

    public bool gameWon = false;

    private Button nextWaveButton;
    private TMP_Text waveRemainingUI;



    void Awake()
    {
        nextWaveButton = GameObject.Find("NextWaveButton").GetComponent<Button>();
    }

    void Start()
    {

        waveRemainingUI = GameObject.Find("WaveTextUI").GetComponent<TMP_Text>();
        waveRemainingUI.text = ("Wave: " + (currentWaveIndex + 1) + "/" + (waves.Length)).ToString();

        InvokeRepeating("CheckIfWon", 0f, .5f);
    }

    void Update()
    {
        CheckForRemainingEnemies();


        if (!waveInProgress && !enemiesRemaining && !gameWon) //if wave is not in progress, and no enemies remain
        {
            nextWaveButton.interactable = true; //since the wave is over, we allow the button to be used again
        }

    }

    public void CheckIfWon() //will run on invoke repeating to avoid bug. Bug happens if last chicken both causes player to win and lose at same time, this way, lose gets applied before win can
    {
        if (((currentWaveIndex) >= waves.Length) && !waveInProgress && !enemiesRemaining && !gameWon)
        {
            gameWon = true;
        }
    }


    public void StartWave()
    {
        if ((currentWaveIndex < waves.Length) && !waveInProgress && !enemiesRemaining)
        {
            waveInProgress = true;
            nextWaveButton.interactable = false; //wave has started, so disable the wave button
            waveRemainingUI.text = ("Wave: " + (currentWaveIndex+1) + "/" + (waves.Length)).ToString();
            Wave currentWave = waves[currentWaveIndex];
            SpawnWave(currentWave);
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    IEnumerator DelayedSpawn(float delay, Wave wave)
    {
        foreach (Enemy enemy in wave.enemy)
        {
            for (int i = 0; i < enemy.enemyCount; i++)
            {
                Instantiate(enemy.enemyPrefab, spawnPoint.position, spawnPoint.rotation); //spawns enemy
                yield return new WaitForSeconds(delay);
            }
        }
        waveInProgress = false;
        Debug.Log("Enum loop?");
    }

    public void SpawnWave(Wave wave)
    {
        StartCoroutine(DelayedSpawn(wave.spawnInterval, wave));
    }

    public void StartNextWave() //connected to button to give player control over wave start
    {
        StartWave();
        currentWaveIndex++;
        Debug.Log("Next wave started!");
    }

    public void CheckForRemainingEnemies() //method to check if enemies are remaining
    {
        GameObject[] numEnemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy");
        if (numEnemiesRemaining.Length >= 1)
        {
            enemiesRemaining = true;
        }
        else
        {
            enemiesRemaining = false;
        }
    }
}
