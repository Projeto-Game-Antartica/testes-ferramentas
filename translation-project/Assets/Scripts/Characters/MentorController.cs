using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentorController : MonoBehaviour {

    public SpriteRenderer dialogPanel;
    public AudioSource audioSource; // beep for localization
    
    private void Start()
    {
        if (Parameters.ACCESSIBILITY)
        {
            audioSource.playOnAwake = true;
            audioSource.Play();
            audioSource.loop = true;
        }
    }

    private void Update()
    {
        if (Parameters.ACCESSIBILITY)
        {
            audioSource.playOnAwake = true;
            if (!audioSource.isPlaying) audioSource.Play();
            audioSource.loop = true;
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }
    }
}
