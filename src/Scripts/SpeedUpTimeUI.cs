using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedUpTimeUI : MonoBehaviour
{
    public float currentTimescale; //gets the current timescale
    private TMP_Text Speed1;
    private TMP_Text Speed2;
    private TMP_Text Speed3;
    // Start is called before the first frame update
    void Start()
    {
        Speed1 = GameObject.Find("1.0Speed").GetComponent<TMP_Text>();
        Speed2 = GameObject.Find("1.5Speed").GetComponent<TMP_Text>();
        Speed3 = GameObject.Find("2.0Speed").GetComponent<TMP_Text>();

        Speed2.gameObject.SetActive(false);
        Speed3.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currentTimescale = Time.timeScale;
    }

    public void SpeedUpTimeButton()
    {
        if (currentTimescale >= 1 && currentTimescale <=3)
        {
            Time.timeScale = 1f;
            Speed1.gameObject.SetActive(true);
            Speed2.gameObject.SetActive(false);
            Speed3.gameObject.SetActive(false);
        }
        if (currentTimescale < 2)
        {
            Time.timeScale = 2f;
            Speed1.gameObject.SetActive(false);
            Speed2.gameObject.SetActive(true);
            Speed3.gameObject.SetActive(false);
        }
        if (currentTimescale == 2)
        {
            Time.timeScale = 3f;
            Speed1.gameObject.SetActive(false);
            Speed2.gameObject.SetActive(false);
            Speed3.gameObject.SetActive(true);
        }
    }
}
