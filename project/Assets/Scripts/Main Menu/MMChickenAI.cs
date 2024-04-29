using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//a seperate script to chickenAI, these chickens dont need all the attributes of an actual chicken
public class MMChickenAI : MonoBehaviour
{
    //private PathAI pathAI;
    public float health = 100f;
    public float speed = 1.0f;
    public int weight = 1;
    private Transform target;
    private Transform chickenPos; //Just used to give "transform" another name, helps with readability
    private Animator chickenAnim;
    public int turnIndex;
    public float threshold = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        target = PathAI.turns[0];

        chickenPos = transform;

        chickenAnim = GetComponent<Animator>();
        chickenAnim.SetBool("Run", true);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - chickenPos.position;
        direction.y = 0.0f; //ignore y axis, will be useful for having multiple chickens if chickens are different sizes
        chickenPos.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(chickenPos.position, target.position) < threshold)
        {
            turnIndex = turnIndex + 1;
            target = PathAI.turns[turnIndex];
            chickenPos.LookAt(target);
        }
    }

    public void ReduceHealth(float damage)
    {
        health -= damage;
        if (health <= 0) //if enemy health drops belo 0, they die
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            ReduceHealth(projectile.damage);
        }
    }
}
