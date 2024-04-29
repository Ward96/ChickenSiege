using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScoringSystem : MonoBehaviour
{
    private PlayerStats playerStatsScript; //create playerStats script object
    public float totalPlayerHealth; //gets the initial total health of the player
    public float score;

    //get the images for the stars
    public Image star1;
    public Image star2;
    public Image star3;

    public string scoreFileName;//string for the file path to save the score to, will be used in inspector

    private WaveManager waveManagerScript;//get the wave manager so that we know when the game is won, and can then write to the file

    // Start is called before the first frame update
    void Start()
    {
        waveManagerScript = GameObject.Find("Spawner").GetComponent<WaveManager>(); //get wave manager script

        playerStatsScript = GameObject.Find("PlaceholderManager").GetComponent<PlayerStats>(); //get the actual script attacked to placeholder manager
        totalPlayerHealth = playerStatsScript.PlayerHP; //get the initial total health of the player
    }

    // Update is called once per frame
    void Update()
    {
        ScoreUpdater();
        CheckIfGameWon();
    }

    private void ScoreUpdater()
    {
        float healthPercentage = playerStatsScript.PlayerHP / totalPlayerHealth; //get the percentage of the remaining health

        if (healthPercentage > 0.85f) //if the pllayer has more that 85%, they get 3 stars
        {
            score = 3;
        }
        else if (healthPercentage > 0.70f)//if the pllayer has more that 70%, they get 2 stars
        {
            score = 2;
            star3.color = new Color32(30, 20, 20, 100);
        }
        else if (healthPercentage > 0.55f)//if the pllayer has more that 55%, they get 1 stars
        {
            score = 1;
            star2.color = new Color32(30, 20, 20, 100);
            star3.color = new Color32(30, 20, 20, 100);
        }
        else //player gets no stars
        {
            score = 0;
            star1.color = new Color32(30, 20, 20, 100);
            star2.color = new Color32(30, 20, 20, 100);
            star3.color = new Color32(30, 20, 20, 100);
        }
    }

    private void CheckIfGameWon() //checks if the game has been won before writing to the file
    {
        if (waveManagerScript.gameWon)
        {
            CheckIfCheating();//since game has been won, check if the player is cheating
        }
    }
    private void CheckIfCheating() //checks if the player has cheats mode on before writing to the file
    {
        if (PlayerPrefs.GetInt("AllowCheats") != 1) //if the player is not cheating
        {
            WriteScoreToFile(); //then write the file
        }
    }

    private void WriteScoreToFile()
    {
        string path = Application.streamingAssetsPath + "/PlayerScores/" + scoreFileName + ".txt";

        if (!File.Exists(path)) //if file does not exist, just make it and write the score to it
        {
            File.WriteAllText(path, score.ToString());
        }
        else //if file already exists, then make sure the achieved score is greater than the stored score before storing it
        {
            //read the existing score from the file
            string existingScoreString = File.ReadAllText(path);
            float existingScore = float.Parse(existingScoreString);

            if (score > existingScore)//compare the existing score with the new score
            {
                //write the new score to the file
                File.WriteAllText(path, score.ToString());
            }
        }
    }
}
