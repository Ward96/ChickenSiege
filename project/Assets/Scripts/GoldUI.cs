using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public TMP_Text goldText;
    private PlayerStats playerStatsScript;
    // Start is called before the first frame update
    void Start()
    {
        playerStatsScript = GameObject.Find("PlaceholderManager").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        SetGoldUI();
    }

    private void SetGoldUI()
    {
        int roundedGold = Mathf.FloorToInt(playerStatsScript.PlayerMoney);
        goldText.text = roundedGold.ToString();
    }
}
