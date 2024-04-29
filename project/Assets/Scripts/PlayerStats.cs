using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int PlayerHP;
    public float PlayerMoney;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCheats();
    }

    private void CheckForCheats() //gives the player 1000 every frame, if cheats are turned on
    {
        if (PlayerPrefs.GetInt("AllowCheats") != 0)
        {
            PlayerMoney = 1000f;
        }
    }
}
