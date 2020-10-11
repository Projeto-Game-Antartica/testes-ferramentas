using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VIDE_Data;

public class SimpleCharacterController : AbstractScreenReader {

    public GameObject character;
    public GameObject inGameOption;
    public GameObject instructionInterface;
    AudioSource audioSource;
    Animator animator;

    private Rigidbody2D rb;
    
    public float SPEED;

    public bool isWalking = false;

    public AudioClip mapLimitClip;

    public AudioClip snowAudioClip;
    public AudioClip woodAudioClip;
    public AudioClip carpetAudioClip;
    public AudioClip grassAudioClip;
    public AudioClip rockAudioClip;
    public AudioClip asphaltAudioClip;
    public AudioClip sandAudioClip;

    public AudioClip impactMetalAudio;
    public AudioClip impactGlassAudio;
    public AudioClip impactWoodAudio;
    public AudioClip impactRockAudio;

    //public ReadableTexts readableTexts;

    // VIDEPlayer attributes

    public string playerName;

    //Reference to our diagUI script for quick access
    public VIDEUIManager diagUI;
    //public QuestChartDemo questUI;

    //Stored current VA when inside a trigger
    public VIDE_Assign inTrigger;

    //DEMO variables for item inventory
    //Crazy cap NPC in the demo has items you can collect
    public List<string> demo_Items = new List<string>();
    public List<string> demo_ItemInventory = new List<string>();

    public string missionNumber;

    public GameObject map;

    public GameObject placaHUD;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        playerName = gameObject.name;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        if (!VD.isActive)
        {
            // character movement
            if (!inGameOption.activeSelf && !instructionInterface.activeSelf && !placaHUD.activeSelf)
            {
                if (movement.magnitude > 0)
                {
                    isWalking = true;

                    if(map.activeSelf)
                    {
                        map.SetActive(false);
                        ReadText("Mapa fechado.");
                    }
                }
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TryInteract();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        audioSource.volume = 1;
        audioSource.pitch = 1;

        if (collision.gameObject.GetComponent<VIDE_Assign>() != null)
        {
            inTrigger = collision.gameObject.GetComponent<VIDE_Assign>();
            TryInteract();
        }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<VIDE_Assign>() != null)
        {
            inTrigger = collision.gameObject.GetComponent<VIDE_Assign>();
            //Debug.Log(inTrigger);
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
                case "wood":
                    audioSource.PlayOneShot(woodAudioClip);
                    break;
                case "carpet":
                    audioSource.PlayOneShot(carpetAudioClip);
                    break;
            }
        }

        if (collision.gameObject.GetComponent<VIDE_Assign>() != null)
        {
            inTrigger = collision.gameObject.GetComponent<VIDE_Assign>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inTrigger = null;
    }

    // Save the position in player prefs
    public void SavePosition(Vector2 position, string missionNumber)
    {
        PlayerPrefs.SetFloat("p_x_" + missionNumber, position.x);

        PlayerPrefs.SetFloat("p_y_" + missionNumber, position.y);

        PlayerPrefs.SetInt("Saved_" + missionNumber, 1);

        PlayerPrefs.Save();
    }

    public Vector3 GetPosition(string missionNumber)
    {
        // Reset, so that the save will be used only once
        PlayerPrefs.SetInt("Saved_"+missionNumber, 0);
        PlayerPrefs.Save();

        return new Vector3(PlayerPrefs.GetFloat("p_x_"+ missionNumber), PlayerPrefs.GetFloat("p_y_" + missionNumber));
    }

    void TryInteract()
    {
        /* Prioritize triggers */
        if (inTrigger)
        {
            diagUI.Interact(inTrigger);
            return;
        }
    }

}
