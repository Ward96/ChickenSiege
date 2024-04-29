using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerStatsUI : MonoBehaviour
{
    private Tile selectedTile;
    private Tower selectedTower;

    //public GameObject towerStats;
    private Button upgrade1;
    private Button upgrade2;
    private Button sellButton;
    private TMP_Text towerTitle;
    private TMP_Text sellValue;

    private TMP_Text camoPurchasedText;
    private TMP_Text metalPurchasedText;

    private TMP_Text camoValueText;
    private TMP_Text metalValueText;

    public Canvas towerStats;

    private TMP_Text targetingText;
    private Button targetingButton;


    //set in inspector, get the image gameobjects that make up tower stats, need to get this so that they can be moved from one side of screen to the other
    public GameObject towerStatsBGOutline;
    public GameObject towerStatsBG;

    public GameObject towerStatsBGOutline_pos2;
    public GameObject towerStatsBG_pos2;

    public Vector3 originalPosition1;
    public Vector3 originalPosition2;
    public Vector3 changedPosition1;
    public Vector3 changedPosition2;

    private PlayerStats playerStatsScript; //needed to make sure the player has enough money to interact with the upgrade buttons

    // Start is called before the first frame update
    void Start()
    {
        //get the UI objects to show details and upgrades of a particular tower
        upgrade1 = GameObject.Find("Upgrade1").GetComponent<Button>();
        upgrade2 = GameObject.Find("Upgrade2").GetComponent<Button>();
        sellButton = GameObject.Find("SellTower").GetComponent<Button>();
        towerTitle = GameObject.Find("TowerTitle").GetComponent<TMP_Text>();

        targetingButton = GameObject.Find("TargetButton").GetComponent<Button>();
        targetingText = GameObject.Find("TargetingText").GetComponent <TMP_Text>(); 

        sellValue = GameObject.Find("TowerSellValueText").GetComponent<TMP_Text>();
        camoPurchasedText = GameObject.Find("CamoPurchased").GetComponent<TMP_Text>();
        metalPurchasedText = GameObject.Find("MetalPurchased").GetComponent<TMP_Text>();

        camoValueText = GameObject.Find("CamoValueText").GetComponent<TMP_Text>();
        metalValueText = GameObject.Find("MetalValueText").GetComponent<TMP_Text>();

        playerStatsScript = GameObject.Find("PlaceholderManager").GetComponent<PlayerStats>();

        camoPurchasedText.gameObject.SetActive(false);
        metalPurchasedText.gameObject.SetActive(false);
        towerStats.gameObject.SetActive(false);

        //get the original positions of the towerstats bg and UI and such
        originalPosition1 = towerStatsBG.transform.position;
        originalPosition2 = towerStatsBGOutline.transform.position;
        //get position of seperate objects used to set a second position
        changedPosition1 = towerStatsBG_pos2.transform.position;
        changedPosition2 = towerStatsBGOutline_pos2.transform.position;
    }

    //update is called once per frame
    void Update()
    {
        towerTargetingUpdater();//call tower target updater so that the stats accurately convey targeting method
    }

    public void OnMouseDown() //get script of tile
    {
        Tile selectedTile = GetComponent<Tile>();
    }

    public void ShowTowerStats(Tile tile, bool towerPlaced, Tower towerScript)
    {
        selectedTile = tile;
        selectedTower = towerScript;
        tile.DisableButtons();
        if (selectedTile.isOnRightSideOfScreen) //if the selected tile is on the right side of the screen, make the UI be on the left side
        {
            towerStatsBG.transform.position = changedPosition1;
            towerStatsBGOutline.transform.position = changedPosition2;
        } else //else, make UI be on right side
        {
            towerStatsBG.transform.position = originalPosition1;
            towerStatsBGOutline.transform.position = originalPosition2;
        }
        if (towerPlaced == true)
        {
            if (selectedTile.currentTowerType == Tile.TowerType.Cannon)
            {
                towerTitle.text = "Cannon Turret";
            }
            if (selectedTile.currentTowerType == Tile.TowerType.Shield)
            {
                towerTitle.text = "Shield Turret";
            }
            if (selectedTile.currentTowerType == Tile.TowerType.Bubble)
            {
                towerTitle.text = "Bubble Turret";
            }
            if (selectedTile.currentTowerType == Tile.TowerType.Musket)
            {
                towerTitle.text = "Musket Turret";
            }
            towerStats.gameObject.SetActive(true);

            sellButton.onClick.RemoveAllListeners();
            sellButton.onClick.AddListener(selectedTile.sellTower);

            int roundedSellValue = Mathf.FloorToInt(selectedTile.sellPrice);
            sellValue.text = "$" + roundedSellValue.ToString();

            camoValueText.text = "$" + tile.camoUpgradePrice.ToString();
            metalValueText.text = "$" + tile.metalUpgradePrice.ToString();

            MetalButtonController();

            CamoButtonController();

            towerTargetingController();

            DisplaySelectedTowerRange();

            selectedTile.PlaceVFX.gameObject.SetActive(true);
        }
    }

    public void HideTowerStats() //connected to button, sets the stats overlay to false and removes the effect showing the current selected tower
    {
        towerStats.gameObject.SetActive(false);
        selectedTile.PlaceVFX.gameObject.SetActive(false);
        selectedTile.cannonRange.gameObject.SetActive(false);
        selectedTile.shieldRange.gameObject.SetActive(false); 
        selectedTile.bubbleRange.gameObject.SetActive(false);
        selectedTile.musketRange.gameObject.SetActive(false);
        selectedTile.EnableButtons();
    }

    private void MetalButtonController()
    {
        if (!selectedTower.canPierceMetal) //if tower cannot pierce, then place metal functionality on button
        {
            if (playerStatsScript.PlayerMoney >= selectedTile.metalUpgradePrice)
            {
                upgrade1.interactable = true;
            }
            else
            {
                upgrade1.interactable = false;
            }
            metalPurchasedText.gameObject.SetActive(false); //no need for purchase text because this hasnt been purchased on this tower
            upgrade1.onClick.RemoveAllListeners();
            upgrade1.onClick.AddListener(selectedTile.GiveMetalPierceStat);
            upgrade1.onClick.AddListener(DisableButton1);
        }
        else //else, metal has been purchased already, so disable the button
        {
            upgrade1.interactable = false;
            metalPurchasedText.gameObject.SetActive(true);
        }
    }
    private void CamoButtonController()
    {
        if (!selectedTower.canDetectCamo) //if tower cannot detect camo, then place camo functionality on button
        {
            if (playerStatsScript.PlayerMoney >= selectedTile.camoUpgradePrice)
            {
                upgrade2.interactable = true;
            }
            else
            {
                upgrade2.interactable = false;
            }
            camoPurchasedText.gameObject.SetActive(false);//no need for purchase text because this hasnt been purchased on this tower
            upgrade2.onClick.RemoveAllListeners();
            upgrade2.onClick.AddListener(selectedTile.GiveCamoStat);
            upgrade2.onClick.AddListener(DisableButton2);
        }
        else //else, camo has been purchased already, so disable the button
        {
            upgrade2.interactable = false;
            camoPurchasedText.gameObject.SetActive(true);
        }
    }

    private void towerTargetingController()
    {
        targetingButton.onClick.RemoveAllListeners();
        targetingButton.onClick.AddListener(selectedTile.SwapTowerTargeting);
        towerTargetingUpdater();
    }
    public void towerTargetingUpdater()
    {
        if (selectedTower != null && targetingText != null)
        {
            if (selectedTower.isTargetingFurthest)
            {
                targetingText.text = "Furthest";
            }
            else
            {
                targetingText.text = "Healthiest";
            }
        }
    }
    private void DisableButton1()
    {
        upgrade1.interactable = false;
        metalPurchasedText.gameObject.SetActive(true);
    }

    private void DisableButton2()
    {
        upgrade2.interactable = false;
        camoPurchasedText.gameObject.SetActive(true);
    }

    private void DisplaySelectedTowerRange()
    {
        if (selectedTile.currentTowerType == Tile.TowerType.Cannon)
        {
            selectedTile.cannonRange.gameObject.SetActive(true);
        }
        if (selectedTile.currentTowerType == Tile.TowerType.Shield)
        {
            selectedTile.shieldRange.gameObject.SetActive(true);
        }
        if (selectedTile.currentTowerType == Tile.TowerType.Bubble)
        {
            selectedTile.bubbleRange.gameObject.SetActive(true);
        }
        if (selectedTile.currentTowerType == Tile.TowerType.Musket)
        {
            selectedTile.musketRange.gameObject.SetActive(true);
        }
    }
}
