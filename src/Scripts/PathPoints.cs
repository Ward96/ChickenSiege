using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoints : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetNext(Transform currPos) {
        if (currPos == null) {
            return transform.GetChild(0);
        }

        return null;
    }
}
