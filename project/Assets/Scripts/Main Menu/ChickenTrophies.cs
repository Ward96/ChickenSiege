using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ChickenTrophies : MonoBehaviour
{
    //get the gameObjects in inspector needed for the trophie
    public GameObject goldenChicken;
    public GameObject silverChicken;
    public GameObject bronzeChicken;
    public GameObject chickenPlatform;


    private string[] scoreFileNames = { "Grass1", "Grass2", "Desert1", "Desert2", "Terracotta1", "Terracotta2" }; //the file names of the scores
    private float totalStars;//a value to keep track of the total number of starrs
    // Start is called before the first frame update
    void Start()
    {
        TallyStars();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTrophie();
    }

    private void TallyStars() //read from the file and count the stars
    {
        for (int x = 0; x < scoreFileNames.Length; x++)
        {
            string path = Application.streamingAssetsPath + "/PlayerScores/" + scoreFileNames[x] + ".txt"; //create the path at x

            if (!File.Exists(path)) //if file does not exist, print
            {
                //File.WriteAllText(path, score.ToString());
                print("file does not exist");
            }
            else
            {
                string scoreString = File.ReadAllText(path);
                totalStars = totalStars + float.Parse(scoreString); //add to the total
            }
        }

    }
    private void DisplayTrophie() //display the chicken trophies in the main menu
    {
        if (totalStars == 18)
        {
            goldenChicken.SetActive(true); //set gold chicken true
            chickenPlatform.SetActive(true);//set table true
        }
        else if (totalStars >= 12)
        {
            silverChicken.SetActive(true);//set silver chicken true
            chickenPlatform.SetActive(true);//set table true
        }
        else if (totalStars >= 6)
        {
            bronzeChicken.SetActive(true);//set bronze chicken true
            chickenPlatform.SetActive(true);//set table true
        }
        else
        {
            print("no chicken award :(");
        }
    }
}
