using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovementController : MonoBehaviour {

    // determined via editor
    public SpriteRenderer spriteRenderer;
    public SphereCollider sphereCollider;

    private AudioSource audioSource;

    public bool rightDirection;
    public bool isWhale;

    public float speed;
    public float TIMER;

    private float hidingTime;
    private float showingTime;

    private void Start()
    {
        hidingTime  = TIMER;
        showingTime = TIMER;
        if (isWhale)    audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        if(isWhale)
        {
            WhaleAnimation();
        }

        SimpleMovement();
    }

    private void SimpleMovement()
    {
        if (rightDirection) // go to right
        {
            if (transform.position.x >= Parameters.RIGHT_LIMIT)
            {
                rightDirection = false;
                spriteRenderer.flipX = !spriteRenderer.flipX; // change direction
            }
            else
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else // go to left
        {
            if (transform.position.x <= Parameters.LEFT_LIMIT)
            {
                rightDirection = true;
                spriteRenderer.flipX = !spriteRenderer.flipX; // change direction
            }
            else
                transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
    }

    private void WhaleAnimation()
    {
        hidingTime -= Time.deltaTime;

        if (hidingTime <= 0)
        {
            spriteRenderer.enabled = true;
            sphereCollider.enabled = true;

            showingTime -= Time.deltaTime;

            if (!audioSource.isPlaying) audioSource.Play();

            if (showingTime <= 0)
            {
                spriteRenderer.enabled = false;
                sphereCollider.enabled = false;
                Parameters.ISWHALEONCAMERA = false;

                hidingTime = TIMER;
                showingTime = TIMER;

                if (audioSource.isPlaying) audioSource.Stop();
            }
        }
    }
}
