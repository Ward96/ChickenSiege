using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChickenAI : MonoBehaviour
{
    //private PathAI pathAI;
    public float health = 100f; //health of chicken, can be changed in inspector
    public float speed = 1.0f;//speed of chicken, can be changed in inspector
    public int weight = 1;//"weight" of chicken, can be changed in inspector, determines how much playerHealth gets reduced whem chicken completes track
    public float chickenValue; //value of the chicken, how much the chicken is worth when slain, set in inspector per chicken
    public Transform target; //target position fort the chicken
    private Transform chickenPos; //Just used to give "transform" another name, helps with readability
    private Animator chickenAnim; //animator for the chicken
    public int turnIndex;
    public float threshold = 0.2f;
    private PlayerStats playerStats;

    private float originalSpeed;
    public float reducedSpeed = 0f; // Adjust as needed
    public float reductionDuration = 3f; // Adjust as needed
    private float reductionTimer = 0f;
    public bool isReducingSpeed = false;

    public GameObject BigBubble; //set in inspector, is the bubble object on the chicken for bubbletower to activate

    public bool isCamoChicken = false; //adjusted in the inspector to set if chicken is a camo chicken
    public bool isMetalChicken = false;//adjusted in the inspector to set if chicken is a metal chicken
    public bool isBossChicken = false;//adjusted in the inspector to set if chicken is a boss chicken

    public float distanceMoved; //tracks the distance the chickens moves, used so that the tower can track the chicken furthest on the track
    private Vector3 lastPosition; //track last position of the chicken to ghet distance moved since last "Update"


    ProjectileTEst projectileScript; //the script of the projectile that hits the chicken

    public GameObject deathEffect; //particle effect to be instantiated when a chicken is slain


    //values needed to determine the value of the chicken, based on the current wave
    private const float maxMultiplier = 1.0f; //the max value of the chicken
    private const float minMultiplier = .10f; //the lowest possible value of hte chicken
    private float waveProgressPercent; //the percentage of the progess in waves, to be used in Lerp
    private float moneyMultiplier; //what Lerp returns
    private WaveManager waveManagerScript; //get the script for the wave manager, used to get the current wave
    private float currentChickenValue;


    // Start is called before the first frame update
    void Start()
    {
        target = PathAI.turns[0]; //initialize target

        chickenPos = transform; //initialize chickenPos for readability

        chickenAnim = GetComponent<Animator>(); //get animator comonent
        chickenAnim.SetBool("Run", true); //set animator to run

        //set var of type PlayerStats to proper compontent, used to give player money when chicken defeated
        playerStats = GameObject.Find("PlaceholderManager").GetComponent<PlayerStats>();

        waveManagerScript = GameObject.Find("Spawner").GetComponent<WaveManager>(); //get wave manager script to be used to determine chicken value

        originalSpeed = speed; //initialize the original speed of the chicken, used to for bubbles

        BigBubble.SetActive(false); //set bubble to false on chicken spawn

        lastPosition = transform.position; //initialize last position in start
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)//need null check for unit test
        {
            MoveTowardsTarget();
        }
        ReduceSpeedForDuration();

        #if !UNITY_TEST
        trackDistanceMoved();//call track distance moved
        #endif

        CalculateMoneyMultiplier();
        currentChickenValue = chickenValue * moneyMultiplier; //get the current value of the chicken based on the total value multiplied by the money multiplier

        Debug.Log("Current Wave Index: " + waveManagerScript.currentWaveIndex);
        Debug.Log("Total Waves: " + waveManagerScript.waves.Length);
    }

    public void MoveTowardsTarget()
    {
        Vector3 direction = target.position - chickenPos.position; //get direction of the target
        direction.y = 0.0f; //ignore y axis, will be useful for having multiple chickens if chickens are different sizes
        chickenPos.Translate(direction.normalized * speed * Time.deltaTime, Space.World); //move the chicken towards the target, based on direction, speed, and relative to the world

        //check if chicken is close enogh to the turn
        if (Vector3.Distance(chickenPos.position, target.position) < threshold)
        {
            turnIndex = turnIndex + 1; //increment turn index
            target = PathAI.turns[turnIndex]; //set the target position to the next point on the path
            chickenPos.LookAt(target);//make chicken loot at the target
        }
    }

    public void ReduceHealth(float damage)//reduces chicken health
    {
        health -= damage;
        if (health <= 0) //if enemy health drops belo 0, they die
        {
            if (deathEffect != null && (PlayerPrefs.GetInt("AllowGore") != 0)) //if death effect not null, and if player has gore turned on
            {
                GameObject copy = Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(copy, 3);
            }
            Destroy(gameObject);

            //calculate the value of the chicken, based on the current wave
            playerStats.PlayerMoney += currentChickenValue; //give the player money based on the current value of the chicken
        }
    }
    private void CalculateMoneyMultiplier()
    {
        waveProgressPercent = (float)(waveManagerScript.currentWaveIndex - 1) / (waveManagerScript.waves.Length - 1); //get the precentage of waves completed, use (float) because the wave index and total waves are integers
        moneyMultiplier = Mathf.Lerp(maxMultiplier, minMultiplier, waveProgressPercent);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            projectileScript = other.GetComponent<ProjectileTEst>();
            CheckMetalStatusForDamage();
        }
    }


    private void ReduceSpeedForDuration() //reduces speed if hit bubble, runs in update but only conditionally on boolean
    {
        if (isReducingSpeed)//check if reducing speed, toggled by a bubble hit
        {
            speed = reducedSpeed; //set speed to reduced speed
            BigBubble.SetActive(true); //activate the bubble effect
            reductionTimer += Time.deltaTime;//increment timere
            if (reductionTimer >= reductionDuration) //check if duration reached
            {
                speed = originalSpeed;//set speed back to original speed
                isReducingSpeed = false;//set reducing speed to false
                reductionTimer = 0f;//reset timer
                BigBubble.SetActive(false);//disable bubble effect
            }
        }
    }

    public void CheckMetalStatusForDamage() //checks if chicken is metal, and if projectile can pierce metal before allowing chicken to do damage
    {
        //check if projectile is not null and the chicken is camouflaged
        if (projectileScript != null && isMetalChicken)
        {
            //check if the tower can detect camouflaged chickens
            if (projectileScript.canPierceMetal)
            {
                ReduceHealth(projectileScript.damage);//if both conditions are met, reduce health
            }
            else
            {
                print("MetalChickenImCrying");//if the tower cannot detect camouflaged chickens, print a message
            }
        }
        else
        {

            ReduceHealth(projectileScript.damage); //if the chicken is not camouflaged or chickenAIScript is null, track and fire
        }
    }

    #if !UNITY_TEST
    public void trackDistanceMoved()//method to track the distance the chicken has moved
    {
        distanceMoved += Vector3.Distance(chickenPos.position, lastPosition); //get distance moves since last frame and add it to distance moved
        lastPosition = chickenPos.position; //update last position to current position
    }
    #endif
}
