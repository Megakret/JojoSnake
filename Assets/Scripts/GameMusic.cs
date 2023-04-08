using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Events.Respawn += ReplayMusic;
        ReplayMusic();
    }
    private void ReplayMusic()
    {
        audioSource.Stop();
        audioSource.Play();
        
    }
}
