using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXAudioController : MonoBehaviour
{
    public AudioClip cannonFire;
    public AudioClip gunFire;
    public AudioClip bowFire;
    public AudioClip bubbleFire;
    public AudioClip turretPlaced;
    public AudioClip turretSold;
    public AudioClip turretUpgraded;
    public AudioClip turretSelection;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fireCannonAudio()
    {
        audioSource.PlayOneShot(cannonFire, .7f);
    }
    public void fireGunAudio()
    {
        audioSource.PlayOneShot(gunFire, .7f);
    }
    public void fireBowAudio()
    {
        audioSource.PlayOneShot(bowFire, .9f);
    }
    public void fireBubbleAudio()
    {
        audioSource.PlayOneShot(bubbleFire, .9f);
    }
    public void playTurretPlaceAudio()
    {
        audioSource.PlayOneShot(turretPlaced, .9f);
    }
    public void playTurretSoldAudio()
    {
        audioSource.PlayOneShot(turretSold, 2f);
    }
    public void playTurretUpgradedAudio()
    {
        audioSource.PlayOneShot(turretUpgraded, .9f);
    }
    public void playTurretSelectAudio()
    {
        audioSource.PlayOneShot(turretSelection, 1.0f);
    }
}
