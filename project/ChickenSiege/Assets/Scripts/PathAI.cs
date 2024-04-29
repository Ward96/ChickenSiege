using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAI : MonoBehaviour
{
    public static Transform[] turns;//creating transform object for all children of "GuidedPath"
    public int children= 10; //adjustable in inspector for reusablility on different maps
    // Start is called before the first frame update
    void Awake()
    {
        StartTurnLogic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartTurnLogic()
    {
        //essentially, set turns[x] = the current position of the current child in loop
        turns = new Transform[children]; //array of size 10 for 10 children
        for (int x = 0; x < turns.Length; x++)
        {
            turns[x] = transform.GetChild(x); //gets index of child and sets turns[x] to that position
        }
    }
}
