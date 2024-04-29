using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNextWaveButton : MonoBehaviour
{
    private WaveManager script;


    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("Spawner").GetComponent<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNextWaveButtonMethod()
    {
        script.StartNextWave();
    }

}
