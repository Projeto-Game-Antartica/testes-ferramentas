using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSoundsController : MonoBehaviour {

    AudioSource audioSource;
    public AudioClip metalImpact;
    public AudioClip glassImpact;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
    
    public void PlayImpactAudio(string tag)
    {
        switch(tag)
        {
            case "Metal":
                audioSource.PlayOneShot(metalImpact);
                break;
            case "Glass":
                audioSource.PlayOneShot(glassImpact);
                break;
        }
    }
}
