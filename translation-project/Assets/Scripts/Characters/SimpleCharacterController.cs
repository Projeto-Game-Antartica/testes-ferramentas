﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VIDE_Data;

public class SimpleCharacterController : AbstractScreenReader {

    public GameObject character;
    public SoundsController soundsController;
    public GameObject inGameOption;
    AudioSource audioSource;
    Animator animator;

    private Rigidbody2D rb;
    
    public float SPEED;

    private bool isWalking = false;

    public AudioClip mapLimitClip;

    public AudioClip snowAudioClip;
    public AudioClip grassAudioClip;
    public AudioClip rockAudioClip;
    public AudioClip asphaltAudioClip;
    public AudioClip sandAudioClip;

    public AudioClip impactMetalAudio;
    public AudioClip impactGlassAudio;
    public AudioClip impactWoodAudio;
    public AudioClip impactRockAudio;


    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        if (!VD.isActive)
        {
            // character movement
            if (!inGameOption.activeSelf)
            {
                if (movement.magnitude > 0)
                    isWalking = true;
                else
                    isWalking = false;

                // check last direction for idle animation: true = right, false = left
                if (movement.x > 0 || movement.y > 0)
                    animator.SetBool("LastDirection", true);
                if (movement.x < 0 || movement.y < 0)
                    animator.SetBool("LastDirection", false);

                // parameters for animator blend tree
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetFloat("Magnitude", movement.magnitude);

                //transform.position = transform.position + movement * SPEED * Time.deltaTime;
                //rb.AddForce(movement * SPEED);
                rb.velocity = movement * SPEED;
            }

            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    animator.SetBool("photographing", true);
            //}
        }
        else
        {
            // runs idle animation when the dialogue is active
            animator.SetFloat("Magnitude", 0);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (!VD.isActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!inGameOption.activeSelf)
                {
                    ReadText("Menu de opções aberto");
                    inGameOption.SetActive(true);
                }
                else
                {
                    ReadText("Menu de opções fechado");
                    inGameOption.SetActive(false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        audioSource.volume = 1;
        audioSource.pitch = 1;

        switch (collision.gameObject.tag)
        {
            case "Metal":
                audioSource.PlayOneShot(impactMetalAudio);
                break;
            case "Glass":
                audioSource.PlayOneShot(impactGlassAudio);
                break;
            case "Wood":
                audioSource.PlayOneShot(impactWoodAudio);
                break;
            case "Rock":
                audioSource.PlayOneShot(impactRockAudio);
                break;
            case "MapLimit":
                audioSource.PlayOneShot(mapLimitClip);
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isWalking && !audioSource.isPlaying)
        {
            audioSource.volume = Random.Range(0.4f, 0.8f);
            audioSource.pitch = Random.Range(0.8f, 1.0f);

            switch(collision.tag)
            {
                case "snow":
                    audioSource.PlayOneShot(snowAudioClip);
                    break;
                case "grass":
                    audioSource.PlayOneShot(grassAudioClip);
                    break;
                case "rock":
                    audioSource.PlayOneShot(rockAudioClip);
                    break;
                case "asphalt":
                    audioSource.PlayOneShot(asphaltAudioClip);
                    break;
                case "sand":
                    audioSource.PlayOneShot(sandAudioClip);
                    break;
            }
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
