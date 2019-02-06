using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovementController : MonoBehaviour {

    // determined via editor
    public SpriteRenderer spriteRenderer;

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
    }

    // Update is called once per frame
    void Update ()
    {
        if(isWhale)
        {
            WhaleAnimation();
        }

        if (rightDirection) // go to right
        {
            if (transform.position.x >= Parameters.RIGHT_LIMIT)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX; // change direction
                rightDirection = false;
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

    void WhaleAnimation()
    {
        hidingTime -= Time.deltaTime;

        if (hidingTime <= 0)
        {
            spriteRenderer.enabled = true;
            showingTime -= Time.deltaTime;

            if (showingTime <= 0)
            {
                spriteRenderer.enabled = false;
                hidingTime = TIMER;
                showingTime = TIMER;
            }
        }
    }
}
