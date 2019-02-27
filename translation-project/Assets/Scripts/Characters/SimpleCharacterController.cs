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

    public float SPEED;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!inGameOption.activeSelf)
                inGameOption.SetActive(true);
            else
                inGameOption.SetActive(false);
        }

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

    void HandleCharacterMovement()
    {
        animator.SetBool("walking", true);
        animator.SetBool("photographing", false);

        WalkingSound();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            //character.transform.position += new Vector3(0, SPEED * Time.deltaTime, 0);
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * SPEED);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            //character.transform.position += new Vector3(0, -SPEED * Time.deltaTime, 0);
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * SPEED);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            //character.transform.position += new Vector3(-SPEED * Time.deltaTime, 0, 0);
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * SPEED);
            character.GetComponent<SpriteRenderer>().flipX = true; // turn left
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //character.transform.position += new Vector3(SPEED * Time.deltaTime, 0, 0);
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * SPEED);
            character.GetComponent<SpriteRenderer>().flipX = false; // turn true
        }
    }

    // Save the position in player prefs
    public void SavePosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("p_x", position.x);
        PlayerPrefs.SetFloat("p_y", position.y);
        PlayerPrefs.SetFloat("p_z", position.z);

        PlayerPrefs.SetInt("Saved", 1);

        PlayerPrefs.Save();
    }

    public Vector3 GetPosition()
    {
        // Reset, so that the save will be used only once
        PlayerPrefs.SetInt("Saved", 0);
        PlayerPrefs.Save();

        return new Vector3(PlayerPrefs.GetFloat("p_x"), PlayerPrefs.GetFloat("p_y"), PlayerPrefs.GetFloat("p_z"));
    }
}
