using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class SimpleCharacterController : MonoBehaviour {

    public GameObject character;
    AudioSource audioSource;
    Animator animator;

    public float SPEED;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (!VD.isActive)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                HandleCharacterMovement();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    animator.SetBool("photographing", true);
                }

                animator.SetBool("walking", false);
            }
        }
        else
        {
            animator.SetBool("walking", false);
        }
        
    }

    void WalkingSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = Random.Range(0.4f, 0.8f);
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
        }
    }

    void HandleCharacterMovement()
    {
        animator.SetBool("walking", true);
        animator.SetBool("photographing", false);

        WalkingSound();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            character.transform.position += new Vector3(0, SPEED * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            character.transform.position += new Vector3(0, -SPEED * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            character.transform.position += new Vector3(-SPEED * Time.deltaTime, 0, 0);
            character.GetComponent<SpriteRenderer>().flipX = true; // turn left
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            character.transform.position += new Vector3(SPEED * Time.deltaTime, 0, 0);
            character.GetComponent<SpriteRenderer>().flipX = false; // turn true
        }
    }
}
