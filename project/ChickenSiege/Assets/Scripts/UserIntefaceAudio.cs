using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UserIntefaceAudio : MonoBehaviour
{
    public AudioClip menuClick;
    public AudioClip turretSelection;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playClickAudio()
    {
        audioSource.PlayOneShot(menuClick, 1.0f);
    }

    public void playTurretSelectAudio()
    {
        audioSource.PlayOneShot(turretSelection, 1.0f);
    }
}
