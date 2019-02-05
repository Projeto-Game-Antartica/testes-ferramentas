using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovementController : MonoBehaviour {

    // determined via editor
    public SpriteRenderer spriteRenderer;
    public float speed;
    public bool rightDirection;
	
	// Update is called once per frame
	void Update () {

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
}
