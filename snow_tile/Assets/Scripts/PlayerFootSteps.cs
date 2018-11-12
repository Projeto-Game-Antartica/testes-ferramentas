using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSteps : MonoBehaviour
{
    
	public AudioSource waterAudio;
	public AudioSource snowAudio;
	public AudioSource iceAudio;

	private CharController chController;

	public bool inWater = false;
	public bool inSnow = false;
	public bool inIce = false;
    
	private string floor;

	// Use this for initialization
	void Start()
	{
		chController = GetComponent<CharController>();

        // os componentes são arrastados através da interface
		// waterAudio = GetComponent<AudioSource>();
		// snowAudio = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{      
		if (chController.walking && !snowAudio.isPlaying && inSnow)
			SnowFootSteps();

		if (chController.walking && !waterAudio.isPlaying && inWater)
			WaterFootSteps();

		if (chController.walking && !iceAudio.isPlaying && inIce)
            iceFootSteps();
	}

	void SnowFootSteps() {
		snowAudio.volume = Random.Range(0.2f, 0.4f);
        snowAudio.pitch = Random.Range(0.8f, 1.2f);
        snowAudio.Play();
	}

	void WaterFootSteps()
	{
		waterAudio.volume = Random.Range(0.2f, 0.4f);
		waterAudio.pitch = Random.Range(0.8f, 1.2f);
		waterAudio.Play();
	}

	void iceFootSteps()
    {
        iceAudio.volume = Random.Range(0.2f, 0.4f);
        iceAudio.pitch = Random.Range(0.8f, 1.2f);
        iceAudio.Play();
    }

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.transform.tag == "snow") {
            inSnow  = true;
            inWater = false;
			inIce = false;
        }
        else if (hit.transform.tag == "water") {
            inSnow  = false;
            inWater = true;
			inIce = false;
        }
		else if (hit.transform.tag == "ice") {
			inSnow = false;
            inWater = false;
			inIce = true;
		}
	}
}
