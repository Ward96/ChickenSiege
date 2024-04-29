using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct MMWave
{
    public int enemyCount;
    public float spawnInterval;
    //public GameObject enemyPrefab;
}

public class MainMenuSpawner : MonoBehaviour //THIS IS AN ALTERNATE FILE TO WAVEMANAGER, ONLY TO BE USED IN THE MAIN MENU TO SPAWN CHICKENS IN THE LEVEL SELECT SCREEN
{
    public Transform spawnPoint; //enemysopawn
    public GameObject enemyPrefab;
    public MMWave[] waves; //array for wave info to adjust in inspector

    private int currentWaveIndex = 0;

    //two booleans used to keep the player from spamming "start wave"
    public bool waveInProgress = false; //boolean to determine if a wave is in progress or not
    public bool enemiesRemaining = false;//boolean to determine if any enemies are remaining

    public bool inMainMenu = false; //boolean used to tell the spawner if we're in the main menu or not. allows the spawner to be reused for alternate purpose without errors

    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    void StartWave()
    {
        if ((currentWaveIndex < waves.Length) && !waveInProgress && !enemiesRemaining)
        {
            waveInProgress = true;
            MMWave currentWave = waves[currentWaveIndex];
            SpawnWave(currentWave);
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    IEnumerator DelayedSpawn(float delay, MMWave wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(delay);
        }
        waveInProgress = false;
    }

    void SpawnWave(MMWave wave)
    {
        StartCoroutine(DelayedSpawn(wave.spawnInterval, wave));
    }


    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void StartNextWave() //connected to button to give player control over wave start
    {
        StartWave();
        currentWaveIndex++;
        Debug.Log("Next wave started!");
    }

}