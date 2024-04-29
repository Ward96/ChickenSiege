using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    //booleans for button logic
    public static bool placingTurret;
    public bool occupied;
    public static bool cannon = false;
    public static bool shield;
    public static bool bubble;
    public static bool musket;

    public bool isPaused = false;//boolean to determine if the game is paused

    //all particle system declarations
    public ParticleSystem PlaceVFX;
    public bool effectEnabled;
    public ParticleSystem cannonRange;
    public ParticleSystem bubbleRange;
    public ParticleSystem shieldRange;
    public ParticleSystem musketRange;

    //all buttons controlled by the script
    private Button shieldButton;
    private Button cannonButton;
    private Button bubbleButton;
    private Button musketButton;

    public PlayerStats playerStats;//script for playerstats

    public bool towerPlaced = false; //boolean to set if a tower is placed, 

    private TowerStatsUI towerStatsScript;//the script for towerstats

    //the sell values
    public float sellPricePercentage = 0.5f;
    public float sellPrice = 0f;

    private GameObject copy;//copy of the tower

    public Tower copyScript;//copy of the script of the placed tower

    private static Tile selectedTile; //track the currently selected tile

    public bool isOnRightSideOfScreen;//determines if tile is on the right side of screen, used to move the tower stats UI so that it does not block selected tower, adjusted in inspector

    private SFXAudioController SFXAudio; //The audio source for all placing, selling sounds from tower

    //text values in the UI of the prices of each tower
    private TMP_Text cannonPriceText;
    private TMP_Text bubblePriceText;
    private TMP_Text musketPriceText;
    private TMP_Text shieldPriceText;


    //new tower placing/upgrade system. replaced booleans with enum, gameobjectarray, and float array
    public enum TowerType { Cannon, Shield, Bubble, Musket}
    public TowerType currentTowerType;
    public GameObject[] towerPrefabs;
    public float[] towerPrices;
    public float camoUpgradePrice = 25;
    public float metalUpgradePrice = 45;
    public int towerIndex;

    //needed for the testrunner tests to work
    public bool isInTestRunner = false;
    private void Awake()
    {
        towerStatsScript = GameObject.Find("TowerStatsUI")?.GetComponent<TowerStatsUI>();

        SFXAudio = GameObject.Find("SFXAudio")?.GetComponent<SFXAudioController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("PlaceholderManager").GetComponent<PlayerStats>();
        cannonButton = GameObject.Find("CannonTowerUI").GetComponent<Button>();
        shieldButton = GameObject.Find("ShieldTowerUI").GetComponent<Button>();
        bubbleButton = GameObject.Find("BubbleTowerUI").GetComponent<Button>();
        musketButton = GameObject.Find("MusketTowerUI").GetComponent<Button>();

        cannonPriceText = GameObject.Find("CannonValueText").GetComponent<TMP_Text>();
        cannonPriceText.text = "$" + towerPrices[0].ToString();
        bubblePriceText = GameObject.Find("BubbleValueText").GetComponent<TMP_Text>();
        bubblePriceText.text = "$" + towerPrices[2].ToString();
        musketPriceText = GameObject.Find("MusketValueText").GetComponent<TMP_Text>();
        musketPriceText.text = "$" + towerPrices[3].ToString();
        shieldPriceText = GameObject.Find("ShieldValueText").GetComponent<TMP_Text>();
        shieldPriceText.text = "$" + towerPrices[1].ToString();

    }

    // Update is called once per frame
    void Update()
    {
        TowerButtonLogic();//track the logic that determiens which button is selected
        UpdateSellPrice();//always track the price of the tower
        ToggleDisableAllOnPause();//disables tile functionality if game gets paused
    }

    public void OnMouseEnter()
    {
        DisplayRangeOnPlacement();//whenever the mouse enters a tile, we want to display the range of the selected tower
    }

    public void OnMouseDown()
    {
        TowerStatsController();
        UpdateTowerIndex();
        PlaceTower(towerIndex); //call PlaceTower
    }
    public void OnMouseExit()
    {
        if (selectedTile != this)//hide VFX if this tile is not selected
        {
            PlaceVFX.gameObject.SetActive(false);
            bubbleRange.gameObject.SetActive(false);
            shieldRange.gameObject.SetActive(false);
            cannonRange.gameObject.SetActive(false);
            musketRange.gameObject.SetActive(false);
        }
    }

    public void swapPlacingTurret()
    {
        placingTurret = !placingTurret;
    }

    public void swapCannon()
    {
        cannon = !cannon;
    }

    public void swapShield()
    {
        shield = !shield;
    }

    public void swapBubble()
    {
        bubble = !bubble;
    }

    public void swapMusket()
    {
        musket = !musket;
    }

    public void sellTower() //method to sell a tower
    {
        print("Sold " + gameObject.name);

        sellPrice = 0f;
        if (towerPlaced)//if a tower is placed, check to see what type of tower is placed, then set the sell price accordingly
        {
            if (currentTowerType == TowerType.Cannon)
            {
                sellPrice = towerPrices[0] * sellPricePercentage;
            }
            else if (currentTowerType == TowerType.Shield)
            {
                sellPrice = towerPrices[1] * sellPricePercentage;
            }
            else if (currentTowerType == TowerType.Bubble)
            {
                sellPrice = towerPrices[2] * sellPricePercentage;
            }
            else if (currentTowerType == TowerType.Musket)
            {
                sellPrice = towerPrices[3] * sellPricePercentage;
            }
        }

        if (copy != null)
        {
            Debug.Log("Destroying tower copy.");
            //destroy the copy of the tower prefab instead of the original tower prefab (duh)
            Destroy(copy);
        }
        else
        {
            Debug.LogWarning("Tower copy is null.");
        }

        occupied = false;

        //update players money based on sell price
        playerStats.PlayerMoney += sellPrice;

        //update tower placed status
        towerPlaced = false;


        if (!isInTestRunner)//if we are not in the test runner, then do these things
        {

            //disable effects
            PlaceVFX.gameObject.SetActive(false);
            selectedTile.cannonRange.gameObject.SetActive(false);
            selectedTile.shieldRange.gameObject.SetActive(false);
            selectedTile.bubbleRange.gameObject.SetActive(false);

            selectedTile = null;

            //hide tower stats UI
            towerStatsScript.HideTowerStats();

            //play the audio
            SFXAudio.playTurretSoldAudio();
        }

    }


    public void GiveCamoStat()//method to give the tower the camo stat
    {
        if (playerStats.PlayerMoney >= camoUpgradePrice && copyScript.canDetectCamo == false) //price of the upgrade
        {
            playerStats.PlayerMoney -= camoUpgradePrice;//subtract money
            copyScript.UpgradeTowerToDetectCamo();//call the method of the script of only the copied tower

            SFXAudio.playTurretUpgradedAudio();//play upgrade noise
        }
    }

    public void GiveMetalPierceStat()//method to give the tower the metal pierce stat
    {
        if (playerStats.PlayerMoney >= metalUpgradePrice && copyScript.canPierceMetal == false) //price of the upgrade
        {
            playerStats.PlayerMoney -= metalUpgradePrice;//subtract money
            copyScript.UpgradeProjectileToPierceMetal();//call the method of the script of only the copied tower

            SFXAudio.playTurretUpgradedAudio(); //play upgrade noise
        }
    }

    public void SwapTowerTargeting() //method to swap the targeting of the tower
    {
        copyScript.swapTargeting(); //call the method of the script of only the copied tower
    }

    private void UpdateSellPrice()//neccessary so that TowerStatsUI can display the sell price in the UI
    {
        if (towerPlaced)
        {
            if (currentTowerType == TowerType.Cannon)
            {
                sellPrice = towerPrices[0] * sellPricePercentage;
            }
            else if (currentTowerType == TowerType.Shield)
            {
                sellPrice = towerPrices[1] * sellPricePercentage;
            }
            else if (currentTowerType == TowerType.Bubble)
            {
                sellPrice = towerPrices[2] * sellPricePercentage;
            }
            else if (currentTowerType == TowerType.Musket)
            {
                sellPrice = towerPrices[3] * sellPricePercentage;
            }
        }
    }

    public void DisableButtons()//method to disable all the buttons, used on pause
    {
        bubble = false;
        shield = false;
        cannon = false;
        musket = false;
        placingTurret = false;

        bubbleButton.gameObject.SetActive(false);
        shieldButton.gameObject.SetActive(false);
        cannonButton.gameObject.SetActive(false);
        musketButton.gameObject.SetActive(false);
    }
    public void EnableButtons()//method to enable all of the buttons
    {
        bubbleButton.gameObject.SetActive(true);
        shieldButton.gameObject.SetActive(true);
        cannonButton.gameObject.SetActive(true);
        musketButton.gameObject.SetActive(true);
    }

    public void PlaceTower(int towerIndex)//method used for placing towers
    {
        if (placingTurret && !occupied && playerStats.PlayerMoney >= towerPrices[towerIndex])//if placing a tower, if no tower is placed, if the placer has enough money, place tower
        {
            copy = Instantiate(towerPrefabs[towerIndex], transform.position, transform.rotation);//create a copy of the prefab
            Transform childTransform = copy.transform.GetChild(0); //get the first child transform
            copyScript = childTransform.GetComponent<Tower>(); //get the Tower component from the first child
            occupied = !occupied;//make the tile occupies
            playerStats.PlayerMoney -= towerPrices[towerIndex];//subtract from the playuers money
            towerPlaced = true;//make the tower placed
            currentTowerType = (TowerType)towerIndex;//set the current tower type of the specific tile
            SFXAudio.playTurretPlaceAudio();//play the audio for placing a tower
        }
    }

    private void UpdateTowerIndex()//updates the towerIndex
    {
        if (cannon)
        {
            towerIndex = 0;
        }
        else if (shield)
        {
            towerIndex = 1;
        }
        else if (bubble)
        {
            towerIndex = 2;
        }
        else if (musket)
        {
            towerIndex = 3;
        }
    }

    private void DisplayRangeOnPlacement()//method to only show the range of the corresponding tower
    {
        if (placingTurret && !occupied)
        {
            PlaceVFX.gameObject.SetActive(true);
        }
        if (placingTurret && cannon && !occupied)
        {
            cannonRange.gameObject.SetActive(true);
        }
        if (placingTurret && bubble && !occupied)
        {
            bubbleRange.gameObject.SetActive(true);
        }
        if (placingTurret && shield && !occupied)
        {
            shieldRange.gameObject.SetActive(true);
        }
        if (placingTurret && musket && !occupied)
        {
            musketRange.gameObject.SetActive(true);
        }
    }

    private void TowerStatsController()//method to handle topwer stats
    {
        if (towerPlaced)
        {
            towerStatsScript.ShowTowerStats(this, towerPlaced, copyScript);

            if (selectedTile != null && selectedTile != this)
            {
                //hide VFX of previously selected tile
                selectedTile.PlaceVFX.gameObject.SetActive(false);
                selectedTile.cannonRange.gameObject.SetActive(false);
                selectedTile.shieldRange.gameObject.SetActive(false);
                selectedTile.bubbleRange.gameObject.SetActive(false);
                selectedTile.musketRange.gameObject.SetActive(false);
            }
            selectedTile = this; //set this tile as the selected tile
            PlaceVFX.gameObject.SetActive(true); //show VFX
            SFXAudio.playTurretSelectAudio();

        }
    }

    public void ToggleDisableAllOnPause()
    {
        if (Time.timeScale == 0f) //if not paused
        {
            if (placingTurret) //and if placing a turret, disable these
            {
                placingTurret = false;
                cannon = false;
                shield = false;
                bubble = false;
                musket = false;
            }
        }
    }

    public void TogglePaused()
    {
        isPaused = !isPaused;
    }

    public void TestRunnerTrue() //method to swap the test runner flag, to be run in test runner
    {
        isInTestRunner = true;
    }

    private void TowerButtonLogic()//method that handles the logic for enabling and disabling buttons
    {
        if (placingTurret && !occupied && cannon) //if these are true, we're placing cannon, so disable all other buttons
        {
            shieldButton.interactable = false;
            bubbleButton.interactable = false;
            musketButton.interactable = false;
        }
        if (placingTurret && !occupied && shield) //if these are true, we're placing shield, so disable all other buttons
        {
            cannonButton.interactable = false;
            bubbleButton.interactable = false;
            musketButton.interactable = false;
        }
        if (placingTurret && !occupied && bubble)//if these are true, we're placing bubble, so disable all other buttons
        {
            cannonButton.interactable = false; //cannon false
            shieldButton.interactable = false; //sheild false
            musketButton.interactable = false;
        }
        if (placingTurret && !occupied && musket)//if these are true, we're placing musket, so disable all other buttons
        {
            cannonButton.interactable = false; //cannon false
            shieldButton.interactable = false; //sheild false
            bubbleButton.interactable = false;
        }
        if (!placingTurret) //if not placing turret, let all buttons be interactable
        {
            shieldButton.interactable = true;
            cannonButton.interactable = true;
            bubbleButton.interactable = true;
            musketButton.interactable = true;
        }
    }
}
