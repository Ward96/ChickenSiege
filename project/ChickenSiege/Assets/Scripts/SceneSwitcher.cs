using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartTerracottaOne()
    {
        SceneManager.LoadScene("Terracotta1");
    }
    public void StartTerracottaTwo()
    {
        SceneManager.LoadScene("Terracotta2");
    }

    public void StartGrassOne()
    {
        SceneManager.LoadScene("Grass1");
    }

    public void StartGrassTwo()
    {
        SceneManager.LoadScene("Grass2");
    }

    public void StartDesertOne()
    {
        SceneManager.LoadScene("Desert1");
    }
    public void StartDesertTwo()
    {
        SceneManager.LoadScene("Desert2");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
