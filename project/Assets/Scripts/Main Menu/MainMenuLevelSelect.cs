using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLevelSelect : MonoBehaviour
{

    public TMP_Text TitleText;
    public Button LevelSelect;
    public Button Settings;
    public Button Tutorial;
    public Button ExitGame;
    public Canvas MainMainMenu;
    public Canvas LevelSelectMenu;
    // Start is called before the first frame update
    void Start()
    {
        LevelSelectMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideMenuMain()
    {
        MainMainMenu.gameObject.SetActive(false);
    }

    public void ShowLS()
    {
        LevelSelectMenu.gameObject.SetActive(true);
    }
    public void HideLS()
    {
        LevelSelectMenu.gameObject.SetActive(false);
    }

    public void ShowMM()
    {
        MainMainMenu.gameObject.SetActive(true);

    }
}
