using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicLooperController : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip loop;

    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = intro;
        audioSource.Play();

        StartCoroutine(StartLoopAfterIntro());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator StartLoopAfterIntro()
    {
        yield return new WaitForSecondsRealtime(intro.length);
        PlayLooped();
    }

    private void PlayLooped()
    {
        audioSource.loop = true;
        audioSource.clip = loop;
        audioSource.Play();
    }
}
