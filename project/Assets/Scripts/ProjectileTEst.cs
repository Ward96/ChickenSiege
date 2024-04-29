using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTEst : MonoBehaviour
{
    public Transform target;
    public bool directionCaptured = false;
    public bool pierce = false;
    public bool isBubble = false;
    public bool canPierceMetal = false;
    public float projectileLifetime = 3f;
    private Vector3 moveDirection;

    public float speed = 70f;
    public float damage = 10f;

    public ChickenAI theTarget;

    void Start()
    {
        Destroy(gameObject, projectileLifetime);


        theTarget = target.gameObject.GetComponent<ChickenAI>();
    }

    public void Seek(Transform towerTarget)
    {
        target = towerTarget;
        if (!directionCaptured && target != null)
        {
            moveDirection = (target.position - transform.position).normalized;
            directionCaptured = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection.y = 0f;
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        if (canPierceMetal)
        {
            print("Pierce shot fired!!");
        }
    }

    void HitTarget(Collider other)
    {
        if (other.CompareTag("Decoration"))//if the target is a decoration, we want to destroy projectile
        {
            Destroy(gameObject);
            print("Decoration hit!");
        }
        if (target && other.CompareTag("Enemy") && !pierce && isBubble) //if target is enemy, the projectile cant pierce, and is a bubble then check chicken status to see if it can be bubbled
        {
            CheckChickenStatusForBubble();
            Destroy(gameObject); //destroy the projectile upon collision with an enemy
        }

        if (target && other.CompareTag("Enemy") && !pierce)//if target is enemy and projectile cant pierce, only destroyu projectile
        {
            Destroy(gameObject); //destroy the projectile upon collision with an enemy
        }

    }

    void OnTriggerEnter(Collider other)
    {
        HitTarget(other);
    }

    public void UpgradeProjectileToPierceMetal()
    {
        canPierceMetal = true;
    }

    public void CheckChickenStatusForBubble() //checks if chicken is metal, and if projectile can pierce metal before allowing chicken to be bubbled
    {
        if(theTarget != null && !theTarget.isBossChicken)//check if the chicken is boss chicken
        {
            //check if projectile is not null and the chicken is camouflaged
            if (theTarget != null && theTarget.isMetalChicken && theTarget.isReducingSpeed == false)
            {
                //check if the tower can detect camouflaged chickens
                if (canPierceMetal && theTarget.isReducingSpeed == false)
                {
                    //if both conditions are met, reduce speed
                    theTarget.isReducingSpeed = true;
                }
                else
                {
                    //if the tower cannot detect camouflaged chickens, print a message
                    print("MetalChickenICantBubbleIT!!!");
                }
            }
            else
            {
                //if the chicken is not camouflaged or chickenAIScript is null, reduce speed
                theTarget.isReducingSpeed = true;
            }
        }
        else
        {
            print("BOSS CHICKEN!!");
        }
    }
    
}
