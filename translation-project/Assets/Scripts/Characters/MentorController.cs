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
}
