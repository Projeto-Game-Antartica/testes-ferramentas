using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour {

    public GameObject panelInstruction;
    public GameObject panelImage;
    public SpriteRenderer cameraOverlay;

    public AudioClip cameraBeep;
    public AudioSource audioSource;
    
    private const float SPEED = 70.0f;
    
    /*
     * Startup Settings
     */
    private void Awake()
    {
        // camera doesnt start at any border
        Parameters.RIGHT_BORDER = false;
        Parameters.LEFT_BORDER  = false;
        Parameters.UP_BORDER    = false;
        Parameters.DOWN_BORDER  = false;
    }

    // Update is called once per frame
    void Update () {

        if(Input.GetKeyDown(KeyCode.F1) && !panelInstruction.activeSelf && !panelImage.activeSelf)
        {
            ReadableTexts.ReadText(ReadableTexts.foto_instructions);
        }

        if(Input.GetKeyDown(KeyCode.F3) && !panelInstruction.activeSelf && !panelImage.activeSelf)
        {
            ReadableTexts.ReadText(ReadableTexts.foto_sceneDescription);
        }

        if (!panelInstruction.activeSelf && !panelImage.activeSelf && !cameraOverlay.enabled)
        {
            HandleCameraMovement(null);
        }
        else if(cameraOverlay.enabled)
        {
            HandleCameraMovement(cameraBeep);
        }

        ActivateInstructionPanel();
    }

    private void ActivateInstructionPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !panelImage.activeSelf)
        {
            if (!panelInstruction.activeSelf)
            {
                panelInstruction.SetActive(true);
                ReadableTexts.ReadText("Painel aberto.");
                ReadableTexts.ReadText(ReadableTexts.foto_instructions);
                GameObject.Find("button-play").GetComponent<UnityEngine.UI.Button>().Select();
            }
        }
    }

    // when camera overlay is active plays the beep audio
    private void HandleCameraMovement(AudioClip beep)
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (beep != null && !audioSource.isPlaying && Parameters.ACCESSIBILITY) audioSource.PlayOneShot(beep);

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
            if (beep != null && !audioSource.isPlaying && Parameters.ACCESSIBILITY) audioSource.PlayOneShot(beep);

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
            if (beep != null && !audioSource.isPlaying && Parameters.ACCESSIBILITY) audioSource.PlayOneShot(beep);

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
            if (beep != null && !audioSource.isPlaying && Parameters.ACCESSIBILITY) audioSource.PlayOneShot(beep);

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
