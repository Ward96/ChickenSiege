using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TestTesting
{
    GameObject chickenPrefab;
    GameObject towerPrefab;
    GameObject cannonBall;

    [SetUp]
    public void Setup()
    {
        //load the prefab names
        chickenPrefab = Resources.Load<GameObject>("ChickenWeak"); 
        towerPrefab = Resources.Load<GameObject>("BronzeCannonTower");
        cannonBall = Resources.Load<GameObject>("CannonBall");
    }

    [UnityTest]
    public IEnumerator LoadSceneTest()//tests that a scene loads correctly
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(1f);

        Assert.IsTrue(SceneManager.GetActiveScene().name == "Grass1");
    }


    [UnityTest]
    public IEnumerator FindFurthestEnemyWithEnemiesReturnsFurthestEnemy()
    {

        SceneManager.LoadScene("Grass1");

        yield return new WaitForSeconds(0.5f); 
        GameObject enemy1 = Object.Instantiate(chickenPrefab);
        enemy1.GetComponent<ChickenAI>().distanceMoved = 2f;
        enemy1.transform.position = new Vector3(2, 0, 0);
        yield return new WaitForSeconds(0.5f);

        GameObject enemy2 = Object.Instantiate(chickenPrefab);
        enemy2.GetComponent<ChickenAI>().distanceMoved = 5f;
        enemy2.transform.position = new Vector3(5, 0, 0);
        yield return new WaitForSeconds(0.5f);

        GameObject bronzeTower = Object.Instantiate(towerPrefab);
        bronzeTower.transform.position = new Vector3(0, 0, 0);
        bronzeTower.GetComponentInChildren<Tower>().TestRunnerTrue();//swap the test runner boolean
        bronzeTower.GetComponentInChildren<Tower>().FindFurthestEnemy();

        yield return new WaitForSeconds(3f);

        Assert.AreEqual(enemy2.transform, bronzeTower.GetComponentInChildren<Tower>().targetEnemy);

    }

    [UnityTest]
    public IEnumerator FindHealthiestEnemyWithEnemiesReturnsHealthiestEnemy()
    {

        SceneManager.LoadScene("Grass1");

        yield return new WaitForSeconds(0.5f);
        GameObject enemy1 = Object.Instantiate(chickenPrefab);
        enemy1.GetComponent<ChickenAI>().health = 2f;
        enemy1.transform.position = new Vector3(2, 0, 0);
        yield return new WaitForSeconds(0.5f);

        GameObject enemy2 = Object.Instantiate(chickenPrefab);
        enemy2.GetComponent<ChickenAI>().health = 5f;
        enemy2.transform.position = new Vector3(2, 0, 0);
        yield return new WaitForSeconds(0.5f);

        GameObject bronzeTower = Object.Instantiate(towerPrefab);
        bronzeTower.transform.position = new Vector3(0, 0, 0);
        bronzeTower.GetComponentInChildren<Tower>().TestRunnerTrue();//swap the test runner boolean
        bronzeTower.GetComponentInChildren<Tower>().FindHealthiestEnemy();

        yield return new WaitForSeconds(1f);

        Assert.AreEqual(enemy2.transform, bronzeTower.GetComponentInChildren<Tower>().targetEnemy);

    }

    [UnityTest]
    public IEnumerator FindFurthestEnemyNoEnemiesReturnsNull()
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject bronzeTower = Object.Instantiate(towerPrefab);
        bronzeTower.transform.position = new Vector3(0, 0, 0);
        bronzeTower.GetComponentInChildren<Tower>().TestRunnerTrue();//swap the test runner boolean
        //bronzeTower.GetComponentInChildren<Tower>().FindFurthestEnemy();

        yield return new WaitForSeconds(1f);

        Assert.IsNull(bronzeTower.GetComponentInChildren<Tower>().targetEnemy);

    }

    [UnityTest]
    public IEnumerator SellTowerSellPriceCalculatedCorrectly()//tests that the sell price of the tower is calculated correctly
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject theTile = GameObject.Find("TileParent");
        theTile.GetComponent<Tile>().towerPrices = new float[] { 25, 0, 0, 0 }; //set cannon price to 25
        theTile.GetComponent<Tile>().currentTowerType = Tile.TowerType.Cannon;
        theTile.GetComponent<Tile>().towerPlaced = true;

        theTile.GetComponent<Tile>().TestRunnerTrue();
        theTile.GetComponent<Tile>().sellTower();

        float expectedSellPrice = 12.5f;//the expected price should be 12.5, half of the price of the cannon
        Assert.AreEqual(expectedSellPrice, theTile.GetComponent<Tile>().sellPrice);

       
    }

    [UnityTest]
    public IEnumerator SellTowerMoneyIncreasedAfterSelling()//check that the players money geets increased properly when the tower is sold
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject theTile = GameObject.Find("TileParent");
        GameObject theManager = GameObject.Find("PlaceholderManager");

        theManager.GetComponent<PlayerStats>().PlayerMoney = 100; //set the player money to 100

        theTile.GetComponent<Tile>().towerPrices = new float[] { 25, 0, 0, 0 }; //set cannon price to 25
        theTile.GetComponent<Tile>().currentTowerType = Tile.TowerType.Cannon;
        theTile.GetComponent<Tile>().towerPlaced = true;

        theTile.GetComponent<Tile>().TestRunnerTrue();
        theTile.GetComponent<Tile>().sellTower();


        float expectedMoney = 112.5f;//the expected sell proice should be 12.5, half of the price of the cannon
        Assert.AreEqual(expectedMoney, theManager.GetComponent<PlayerStats>().PlayerMoney);
    }

    [UnityTest]
    public IEnumerator StartWaveSetsWaveInProgress()//checks when a wave is started if the waveInProigress boolean gets flipped
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject theWaveManager = GameObject.Find("Spawner");

        theWaveManager.GetComponent<WaveManager>().StartWave();

        Assert.IsTrue(theWaveManager.GetComponent<WaveManager>().waveInProgress);
    }

    [Test]
    public void ReduceHealthTest() //testing the reducehealth method in chickenAI
    {
        GameObject gameObject = new GameObject(); //create gameobject
        ChickenAI chickenAI = gameObject.AddComponent<ChickenAI>(); //attach the chickenAI script to new gameobject

        chickenAI.health = 100; //set the chickens initial health
        chickenAI.ReduceHealth(20f);//call reduce health, reduce by 20

        Assert.AreEqual(80, chickenAI.health); // Assert that health is reduced by the correct amount

        Object.Destroy(gameObject); // Destroy
    }

    [UnityTest]
    public IEnumerator TargetAssignedAndDirectionCapturedTest() //testing the seek method in projectile script
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject enemy1 = Object.Instantiate(chickenPrefab); //spawn target
        enemy1.transform.position = new Vector3(2, 0, 0);
        yield return new WaitForSeconds(0.3f);

        GameObject projectile = Object.Instantiate(cannonBall); //spawn projectile
        projectile.transform.position = new Vector3(10, 0, 0);

        projectile.GetComponent<ProjectileTEst>().Seek(enemy1.transform); //call seek and send the position of enemy1

        yield return new WaitForSeconds(0.2f);
        //assert
        Assert.AreEqual(enemy1.transform, projectile.GetComponent<ProjectileTEst>().target); //check if target is correctly assigned
        Assert.IsTrue(projectile.GetComponent<ProjectileTEst>().directionCaptured); //check if direction is captured
    }

    [UnityTest]
    public IEnumerator CheckChickenStatusForBubble_MetalChicken_ReduceSpeedIfMetalAndCanPierceMetal_Test() //tests the "checkchickenstatusforbubble" script, in the case that the chicken is a metal chicken, and the projectile can pierce metal
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(01f);

        GameObject enemy1 = Object.Instantiate(chickenPrefab); //spawn chicken
        enemy1.transform.position = new Vector3(2, 0, 0);
        enemy1.GetComponent<ChickenAI>().isMetalChicken = true; //make the chicken metal
        enemy1.GetComponent<ChickenAI>().reductionDuration = 10f; //make the duration longer


        GameObject projectile = Object.Instantiate(cannonBall); //spawn projectile
        projectile.transform.position = new Vector3(2, 0, 0);
        projectile.GetComponent<ProjectileTEst>().canPierceMetal = true; //allow the projectile to pierce metal
        projectile.GetComponent<ProjectileTEst>().isBubble = true; //give the cannon ball the bubble property

        projectile.GetComponent<ProjectileTEst>().Seek(enemy1.transform); //call seek and send the position of enemy1

        yield return new WaitForSeconds(1f);

        Assert.IsTrue(enemy1.GetComponent<ChickenAI>().isReducingSpeed); //check if isReducingSpeed is set to true
    }

    [UnityTest]
    public IEnumerator CheckChickenStatusForBubble_MetalChicken_ReduceSpeedIfMetalAndCannotPierceMetal_Test() //tests the "checkchickenstatusforbubble" script, in the case that the chicken is a metal chicken, and the projectile CANNOT pierce metal
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(01f);

        GameObject enemy1 = Object.Instantiate(chickenPrefab); //spawn chicken
        enemy1.transform.position = new Vector3(2, 0, 0);
        enemy1.GetComponent<ChickenAI>().isMetalChicken = true; //make the chicken metal
        enemy1.GetComponent<ChickenAI>().reductionDuration = 10f; //make the duration longer


        GameObject projectile = Object.Instantiate(cannonBall); //spawn projectile
        projectile.transform.position = new Vector3(2, 0, 0);
        projectile.GetComponent<ProjectileTEst>().canPierceMetal = false; //dontallow the projectile to pierce metal
        projectile.GetComponent<ProjectileTEst>().isBubble = true; //give the cannon ball the bubble property

        projectile.GetComponent<ProjectileTEst>().Seek(enemy1.transform); //call seek and send the position of enemy1

        yield return new WaitForSeconds(1f);

        Assert.IsFalse(enemy1.GetComponent<ChickenAI>().isReducingSpeed); //check if isReducingSpeed is set to false
    }

    [UnityTest]
    public IEnumerator ChickenMovesThroughPathTest() //tests the "checkchickenstatusforbubble" script, in the case that the chicken is a metal chicken, and the projectile CANNOT pierce metal
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject enemy1 = Object.Instantiate(chickenPrefab); //spawn chicken
        enemy1.transform.position = new Vector3(-10, 0, 0);
        yield return new WaitForSeconds(5f);//give the chicken time to reach a turn

        Assert.IsTrue(enemy1.GetComponent<ChickenAI>().turnIndex > 0);
    }
    [UnityTest] 
    public IEnumerator PlaceTowerReducesPlayersMoneyTest() //tests the "checkchickenstatusforbubble" script, in the case that the chicken is a metal chicken, and the projectile CANNOT pierce metal
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject theTile = GameObject.Find("TileParent");
        GameObject theManager = GameObject.Find("PlaceholderManager");

        theManager.GetComponent<PlayerStats>().PlayerMoney = 100; //set the player money to 100

        theTile.GetComponent<Tile>().towerPrices = new float[] { 25, 0, 0, 0 }; //set cannon price to 25
        yield return new WaitForSeconds(0.5f);
        theTile.GetComponent<Tile>().swapPlacingTurret(); //have to be placing a turret for placetower to work
        theTile.GetComponent<Tile>().PlaceTower(0);//place a tower
        yield return new WaitForSeconds(0.5f);

        float expectedMoney = 75f;//the expected sell proice should be 12.5, half of the price of the cannon
        Assert.AreEqual(expectedMoney, theManager.GetComponent<PlayerStats>().PlayerMoney);
    }

    [UnityTest]
    public IEnumerator StartWaveSpawnsChickens()//checks when a wave is started if the waveInProigress boolean gets flipped
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject theWaveManager = GameObject.Find("Spawner");

        theWaveManager.GetComponent<WaveManager>().StartWave();
        yield return new WaitForSeconds(3f);

        GameObject obj = GameObject.Find("ChickenWeak(Clone)");//find the chicken clone

        Assert.IsNotNull(obj);
    }

    [UnityTest]
    public IEnumerator CheckThatCannonButtonSwapsCannonBoolean()//checks when a wave is started if the waveInProigress boolean gets flipped
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject theButton = GameObject.Find("CannonTowerUI");//find the cannon button
        theButton.GetComponent<Button>().onClick.Invoke();//click the cannon button

        GameObject theTile = GameObject.Find("TileParent");//get the tile object


        Assert.IsTrue(Tile.cannon);
    }
    [UnityTest]
    public IEnumerator CheckThatCannonButtonSwapsPlacingTurretBoolean()//checks when a wave is started if the waveInProigress boolean gets flipped
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject theButton = GameObject.Find("CannonTowerUI");//find the cannon button
        theButton.GetComponent<Button>().onClick.Invoke();//click the cannon button

        GameObject theTile = GameObject.Find("TileParent");//get the tile object


        Assert.IsTrue(Tile.placingTurret);
    }

    [UnityTest]
    public IEnumerator StartWaveButtonStartsWave()//checks when a wave is started if the waveInProigress boolean gets flipped
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject theButton = GameObject.Find("NextWaveButton");//find the start wave button
        theButton.GetComponent<Button>().onClick.Invoke();//click the start wave button

        GameObject theWaveManager = GameObject.Find("Spawner");//get the spawner object
        yield return new WaitForSeconds(0.2f);
        Assert.IsTrue(theWaveManager.GetComponent<WaveManager>().waveInProgress);//if wave in progress swapped then button m,ustve started wave
    }

    [UnityTest]
    public IEnumerator FastForwardButtonSpeedsUpTIme()//checks to see if the fast forward button speeds up time
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject theButton = GameObject.Find("FastFoward");//find the fast forward button
        theButton.GetComponent<Button>().onClick.Invoke();//click the fast forward button

        yield return new WaitForSeconds(0.2f);
        Assert.IsTrue(Time.timeScale > 1);//if time is over one then time has been sped up
    }
    [UnityTest]
    public IEnumerator PauseGameButtonPausesGame()//checks to see if the pause button pauses the game
    {
        SceneManager.LoadScene("Grass1");
        yield return new WaitForSeconds(0.5f);

        GameObject theButton = GameObject.Find("PauseButtonUI");//find the pause button
        theButton.GetComponent<Button>().onClick.Invoke();//click the pause button
        Assert.IsTrue(Time.timeScale == 0);//if time is 0 then time has been stopped and the game is paused
    }
}
