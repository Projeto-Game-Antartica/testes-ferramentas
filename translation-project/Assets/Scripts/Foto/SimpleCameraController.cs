using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour {
    
    private const float SPEED = 70.0f;

    void Start()
    {
        Debug.Log(GetComponent<Camera>().orthographicSize);
    }
    // Update is called once per frame
    void Update () {
        HandleCameraMovement();
    }

    private void HandleCameraMovement()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x >= Parameters.RIGHT_LIMIT)
            {
                transform.position = new Vector3(Parameters.RIGHT_LIMIT, transform.position.y, Parameters.Z_POSITION);
                Parameters.RIGHT_BORDER = true;
            }
            else
            {
                transform.position += new Vector3(SPEED * Time.deltaTime, 0, 0);
                Parameters.LEFT_BORDER = false;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x <= Parameters.LEFT_LIMIT)
            {
                transform.position = new Vector3(Parameters.LEFT_LIMIT, transform.position.y, Parameters.Z_POSITION);
                Parameters.LEFT_BORDER = true;
}
            else
            {
                transform.position += new Vector3(-SPEED * Time.deltaTime, 0, 0);
                Parameters.LEFT_BORDER = false;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.position.y >= Parameters.UP_LIMIT)
            {
                transform.position = new Vector3(transform.position.x, Parameters.UP_LIMIT, Parameters.Z_POSITION);
                Parameters.UP_BORDER = true;
            }
            else
            {
                transform.position += new Vector3(0, SPEED * Time.deltaTime, 0);
                Parameters.UP_BORDER = false;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.position.y <= Parameters.DOWN_LIMIT)
            {
                transform.position = new Vector3(transform.position.x, Parameters.DOWN_LIMIT, Parameters.Z_POSITION);
                Parameters.DOWN_BORDER = true;
            }
            else
            {
                transform.position += new Vector3(0, -SPEED * Time.deltaTime, 0);
                Parameters.DOWN_BORDER = false;
            }
        }
    }
}
