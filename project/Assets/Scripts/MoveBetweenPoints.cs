using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    [SerializeField] private PathPoints pathPoints;
    private Transform currPos;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        currPos = pathPoints.GetNext(transform);
        transform.position = currPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
