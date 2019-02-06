using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour {

    public GameObject panelInstruction;
    private const float SPEED = 70.0f;

    /*
     * Startup Settings
     */
    private void Awake()
    {
        Parameters.ACCESSIBILITY = true;

        // camera doesnt start at any border
        Parameters.RIGHT_BORDER = false;
        Parameters.LEFT_BORDER = false;
        Parameters.UP_BORDER = false;
        Parameters.DOWN_BORDER = false;
    }

    // Update is called once per frame
    void Update () {
        if (!panelInstruction.activeSelf)
        {
            HandleCameraMovement();
        }
        ActivateInstructionPanel();
    }

    private void ActivateInstructionPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!panelInstruction.activeSelf)
            {
                panelInstruction.SetActive(true);
                GameObject.Find("button-play").GetComponent<UnityEngine.UI.Button>().Select();
            }
        }
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
