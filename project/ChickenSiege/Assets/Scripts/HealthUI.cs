using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public TMP_Text healthText;
    private PlayerStats PlayerStats;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStats = GameObject.Find("PlaceholderManager").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = PlayerStats.PlayerHP.ToString();
    }
    void OnTriggerEnter(Collider other)
    {
        SetHealthUI(other);
    }

    private void SetHealthUI(Collider other)
    {
        ChickenAI ChickenScript = other.GetComponent<ChickenAI>();
        PlayerStats.PlayerHP = PlayerStats.PlayerHP - ChickenScript.weight;
        Destroy(other.gameObject);
    }
}


