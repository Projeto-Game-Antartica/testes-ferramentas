using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VIDE_Data;

public class SimpleCharacterController : MonoBehaviour {

    public GameObject character;
    public SoundsController soundsController;
    public GameObject inGameOption;
    AudioSource audioSource;
    Animator animator;

    // cant be too high 
    public float SPEED;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!inGameOption.activeSelf)
                inGameOption.SetActive(true);
            else
                inGameOption.SetActive(false);
        }

        if (!VD.isActive)
        {
            // character movement
            if (!inGameOption.activeSelf)
            {
                if (movement.magnitude > 0) WalkingSound();

                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetFloat("Magnitude", movement.magnitude);
                //animator.SetBool("photographing", false);
                transform.position = transform.position + movement * SPEED *  Time.deltaTime;
            }

            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    animator.SetBool("photographing", true);
            //}
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Metal"))
        {
            soundsController.PlayImpactAudio("Metal");
        }
        else if (collision.gameObject.tag.Equals("Glass"))
        {
            soundsController.PlayImpactAudio("Glass");
        }
    }

    void WalkingSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = Random.Range(0.4f, 0.8f);
            audioSource.pitch = Random.Range(0.8f, 1.0f);
            audioSource.Play();
        }
    }

    // Save the position in player prefs
    public void SavePosition(Vector2 position)
    {
        PlayerPrefs.SetFloat("p_x", position.x);
        PlayerPrefs.SetFloat("p_y", position.y);

        PlayerPrefs.SetInt("Saved", 1);

        PlayerPrefs.Save();
    }

    public Vector3 GetPosition()
    {
        // Reset, so that the save will be used only once
        PlayerPrefs.SetInt("Saved", 0);
        PlayerPrefs.Save();

        return new Vector3(PlayerPrefs.GetFloat("p_x"), PlayerPrefs.GetFloat("p_y"));
    }
}
