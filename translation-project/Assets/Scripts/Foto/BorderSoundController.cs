using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderSoundController : MonoBehaviour {

    public AudioSource audioSource;
	
	// Update is called once per frame
	void Update ()
    {
        if (Parameters.ACCESSIBILITY)
        {
            PlayBorderSound();
        }
	}

    /*
     * Not proud of this piece of code, but works.
     * Check if the camera hitted any border, play the clip and set the parameter to false because the clip
     * would loop until the camera leaves the border
     */
    void PlayBorderSound()
    {
        if (Parameters.UP_BORDER)
        {
            audioSource.Play();
            Parameters.UP_BORDER = false;
        }
        else if (Parameters.DOWN_BORDER)
        {
            audioSource.Play();
            Parameters.DOWN_BORDER = false;
        }
        else if (Parameters.RIGHT_BORDER)
        {
            audioSource.Play();
            Parameters.RIGHT_BORDER = false;
        }
        else if (Parameters.LEFT_BORDER)
        {
            audioSource.Play();
            Parameters.LEFT_BORDER = false;
        }
    }
}
