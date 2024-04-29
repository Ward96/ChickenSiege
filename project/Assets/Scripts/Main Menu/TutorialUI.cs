using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public GameObject[] pages;
    private int currentIndex = 0;
    public TMP_Text pageNumberText;

    public Canvas TutorialMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextItem()
    {
        currentIndex = (currentIndex + 1) % pages.Length;
        ShowItem(currentIndex);
    }

    public void PreviousItem()
    {
        currentIndex = (currentIndex - 1 + pages.Length) % pages.Length;
        ShowItem(currentIndex);
    }

    private void ShowItem(int index)
    {
        //disable all items
        foreach (var item in pages)
        {
            item.SetActive(false);
        }

        //enable the selected item
        pages[index].SetActive(true);

        pageNumberText.text = "Page: " + (index + 1) + "/" + pages.Length;
    }

    public void ShowTutorialMenu()
    {
        TutorialMenu.gameObject.SetActive(true);//set the tutorial canvas to true to show menu
        ShowItem(currentIndex);
    }
    public void HideTutorialMenu()
    {
        TutorialMenu.gameObject.SetActive(false);//set the tutorial canvas to false to hide menu
    }

}
