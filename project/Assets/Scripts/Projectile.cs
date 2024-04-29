using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public float damage = 10f;

    public void Seek (Transform _target)
    {
        target = _target;
        Debug.Log("Seek Called");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            //Destroy(gameObject);
            return;
        }
            Vector3 direction = target.position - transform.position;
        Debug.Log("Move direction captured: " + direction);
        float distanceThisFrame = speed * Time.deltaTime;

            if (direction.magnitude <= distanceThisFrame)
            {
                HitTarget();
            }

            transform.Translate(direction.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        //ChickenAI chicken = 
        // Add damage dealing logic or effects here
        //Destroy(gameObject);
    }
}
