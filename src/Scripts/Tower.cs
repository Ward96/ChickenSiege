using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float trackingRadius = 10f;

    public Transform targetEnemy;

    public float rateOfFire = 1f;
    private float fireCountdown = 0f;
    //public float damage = 10f;

    public GameObject projectilePrefab;
    public Transform exitLocation;

    public ProjectileTEst projectileScript;

    public ChickenAI chickenAIScript;

    public bool canDetectCamo = false;
    public bool canPierceMetal = false;

    public bool isTargetingStrongest = false; //determines if the tower is targeting strongest, unused but will be used if more methods of targeting get added
    public bool isTargetingFurthest = true;//determines if the tower is targeting furthest

    public ParticleSystem shootEffect; //effect for firing

    public enum TowerType { Cannon, Shield, Bubble, Musket }; //used to for audio selection
    public TowerType currentTowerType; //set in inspector
    private SFXAudioController SFXAudio; //ythe audio source for all "fire" sounds from tower

    //needed for the testrunner tests to work
    public bool isInTestRunner = false;

    // Start is called before the first frame update
    void Start()
    {
        isTargetingFurthest = true;
        SFXAudio = GameObject.Find("SFXAudio").GetComponent<SFXAudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        checkTargeting();

    }

    private void Shoot()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, exitLocation.position, exitLocation.rotation);
        projectileScript = projectileObject.GetComponent<ProjectileTEst>(); //get the script of the projectile

        CheckMetalStat(); //check if the projectile can pierce metal

        projectileScript.Seek(targetEnemy); //use seek method in projectileScript

        if (shootEffect != null)
        {
            shootEffect.Stop();
            shootEffect.Play();
        }

        FireAudioController();
    }

    private void FireAudioController()
    {
        if (currentTowerType == TowerType.Cannon)
        {
            SFXAudio.fireCannonAudio();
        }
        else if (currentTowerType == TowerType.Musket)
        {
            SFXAudio.fireGunAudio();
        }
        else if (currentTowerType == TowerType.Shield)
        {
            SFXAudio.fireBowAudio();
        }
        else if (currentTowerType == TowerType.Bubble)
        {
            SFXAudio.fireBubbleAudio();
        }
    }

    public void FindFurthestEnemy() //method to find the furthest enemy along the path, within the towers range
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //create array to store all current enemies
        float maxDistanceMoved = 0; //initialize
        Transform furthestEnemy = null; //initialize, no enemy furthest away

        foreach (GameObject enemy in enemies) //iterate through each enemy in the enemy array
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position); //get the distance between tower and enemy
            if (distanceToEnemy <= trackingRadius) //check if the enemy is within the tower's range
            {
                chickenAIScript = enemy.GetComponent<ChickenAI>(); //get the ChickenAI script of the enemy
                if (chickenAIScript != null && chickenAIScript.distanceMoved > maxDistanceMoved && (!chickenAIScript.isCamoChicken || canDetectCamo)) //check that the CHickenAi script exists and the enemy has moved further than previously checked enemy in array
                {
                    maxDistanceMoved = chickenAIScript.distanceMoved; //update maximum distance moved
                    furthestEnemy = enemy.transform; //set furthest enemy to the enemy checked
                }
            }
        }
        targetEnemy = furthestEnemy; //assign the furthest enemy as the target enemy

        if (!isInTestRunner)//if we are not in the test runner, then track and fire
        {
            TrackAndFire();
        }
    }

    public void FindHealthiestEnemy()//method to find the healthiest enemy along the path, within the towers range
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float maxHealth = 0; //initialize
        Transform healthiestEnemy = null;//initialize, no enemy healthiest yet

        foreach (GameObject enemy in enemies)//iterate through each enemy in the enemy array
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);//get the distance between tower and enemy
            if (distanceToEnemy <= trackingRadius) //check if the enemy is within the tower's range
            {
                chickenAIScript = enemy.GetComponent<ChickenAI>(); //get the ChickenAI script of the enemy
                if (chickenAIScript != null && chickenAIScript.health > maxHealth && (!chickenAIScript.isCamoChicken || canDetectCamo))
                {
                    maxHealth = chickenAIScript.health;//update maximum distance moved
                    healthiestEnemy = enemy.transform;//set furthest enemy to the enemy checked
                }
            }
        }
        targetEnemy = healthiestEnemy; //assign the healthiest enemy as the target enemy

        if (!isInTestRunner)//if we are not in the test runner, then track and fire
        {
            TrackAndFire();
        }
    }

    public void UpgradeTowerToDetectCamo() //method to upgrade the tower to detect camo, linked to a button in towerstatsUI
    {
        canDetectCamo = true;
    }

    public void UpgradeProjectileToPierceMetal()//method to upgrade the tower to pierce, linked to a button in towerstatsUI
    {
        canPierceMetal = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRadius);
    }

    public void TrackAndFire() //method to track target enemy and fire projectile at enemy
    {
        if (targetEnemy != null) //check if theres an enemy to track
        {
            Vector3 direction = targetEnemy.position - transform.position; //calculate direction
            direction.y = 0f; //ignore y axis so turret always shoots straight
            Quaternion lookRotation = Quaternion.LookRotation(direction); //calculate rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f); //smoothly rotate the tower

            Debug.DrawLine(transform.position, targetEnemy.position, Color.green); //draw a green line to the target

            if (fireCountdown <= 0f) //check if time to fire
            {
                Shoot();//call shoot method to shoot enemy
                fireCountdown = 1f / rateOfFire; //reset fire coundown based on rateof fire
            }
        }

        fireCountdown = fireCountdown - Time.deltaTime;//decrease the fire coundown
    }
    

    private void CheckMetalStat() //checks if the projectile can pierce metal and set it true in projectile script
    {
        if (canPierceMetal)
        {
            projectileScript.canPierceMetal = true;
        }
    }

    private void checkTargeting() //targeting method. 
    {
        if (isTargetingFurthest)
        {
            FindFurthestEnemy();
        } else
        {
            FindHealthiestEnemy();
        }
    }
    public void swapTargeting()//swaps between targeting firthest and healthiest enemy, linked to a button
    {
        isTargetingFurthest = !isTargetingFurthest;
    }

    public void TestRunnerTrue() //method to swap the test runner flag, to be run in test runner
    {
        isInTestRunner = true;
    }

}

