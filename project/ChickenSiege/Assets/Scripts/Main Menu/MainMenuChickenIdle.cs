using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuChickenIdle : MonoBehaviour
{
    private Animator chickenAnim;
    float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        chickenAnim = GetComponent<Animator>();
        InvokeRepeating("PlayAnimEat", 3f, 10.0f);
        InvokeRepeating("StopAnim", 6f, 10.0f);
        InvokeRepeating("PlayAnimTurnHead", 7f, 10.0f);
        InvokeRepeating("StopAnim", 9f, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayAnimEat()
    {
        chickenAnim.SetTrigger("Eat");
    }

    void PlayAnimTurnHead()
    {
        chickenAnim.SetTrigger("Turn Head");
    }

    void StopAnim()
    {
        chickenAnim.ResetTrigger("Eat");
        chickenAnim.ResetTrigger("Turn Head");
    }
}
