using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class ScoreFileReader : MonoBehaviour
{

    //get the images for the stars
    public Image star1;
    public Image star2;
    public Image star3;

    public string scoreFileName;//string for the file path to save the score to, will be used in inspector

    // Start is called before the first frame update
    void Start()
    {
        UpdateStars();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStars();
    }

    private void UpdateStars()
    {
        string path = Application.streamingAssetsPath + "/PlayerScores/" + scoreFileName + ".txt";

        if (!File.Exists(path)) //if file does not exist, just make it and write the score to it
        {
            print("file does not exist");
            star1.color = new Color32(30, 20, 20, 100);
            star2.color = new Color32(30, 20, 20, 100);
            star3.color = new Color32(30, 20, 20, 100);
        }
        else //if file already exists, then make sure the achieved score is greater than the stored score before storing it
        {
            //read the existing score from the file
            string scoreString = File.ReadAllText(path);
            float score = float.Parse(scoreString);

            if (score == 3) //if the pllayer has more that 85%, they get 3 stars
            {
                //score = 3;
            }
            else if (score == 2)//if the pllayer has more that 70%, they get 2 stars
            {
                star3.color = new Color32(30, 20, 20, 100);
            }
            else if (score == 1)//if the pllayer has more that 55%, they get 1 stars
            {
                star2.color = new Color32(30, 20, 20, 100);
                star3.color = new Color32(30, 20, 20, 100);
            }
            else //player gets no stars
            {
                star1.color = new Color32(30, 20, 20, 100);
                star2.color = new Color32(30, 20, 20, 100);
                star3.color = new Color32(30, 20, 20, 100);
            }
        }
    }

    
}
