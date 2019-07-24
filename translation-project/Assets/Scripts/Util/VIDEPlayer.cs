using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VIDE_Data;

public class VIDEPlayer : MonoBehaviour
{
    public string playerName = "Pesquisador";

    //Reference to our diagUI script for quick access
    public VIDEUIManager diagUI;
    //public QuestChartDemo questUI;
    
    //Stored current VA when inside a trigger
    public VIDE_Assign inTrigger;

    //DEMO variables for item inventory
    //Crazy cap NPC in the demo has items you can collect
    public List<string> demo_Items = new List<string>();
    public List<string> demo_ItemInventory = new List<string>();

    //public TMPro.TextMeshProUGUI blinkingTextMentor;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        //if (collision.name.Contains("Mentor"))
        //    blinkingTextMentor.gameObject.SetActive(true);

        Debug.Log("dialog-trigger");
        if (collision.gameObject.GetComponent<VIDE_Assign>() != null)
        {
            inTrigger = collision.gameObject.GetComponent<VIDE_Assign>();
        }
        //TryInteract();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<VIDE_Assign>() != null)
        {
            inTrigger = collision.gameObject.GetComponent<VIDE_Assign>();
        }
        TryInteract();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        inTrigger = null;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inTrigger = null;
        Debug.Log(collision.name);
        //blinkingTextMentor.gameObject.SetActive(false);
    }

    void OnTriggerExit()
    {
        inTrigger = null;
        //blinkingTextMentor.gameObject.SetActive(false);
    }

    void Start()
    {
        /*
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        */
    }

    void Update()
    {

        //Interact with NPCs when pressing E
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        if (VD.isActive && Input.GetKey(KeyCode.Escape))
            diagUI.EndDialogue(VD.nodeData);


        //Hide/Show cursor
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = !Cursor.visible;
            if (Cursor.visible)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
        */
    }

    //Casts a ray to see if we hit an NPC and, if so, we interact
    void TryInteract()
    {
        /* Prioritize triggers */

        if (inTrigger)
        {
            diagUI.Interact(inTrigger);
            return;
        }

        /* If we are not in a trigger, try with raycasts */
        /*
        RaycastHit rHit;

        if (Physics.Raycast(transform.position, transform.forward, out rHit, 2))
        {
            //Lets grab the NPC's VIDE_Assign script, if there's any
            VIDE_Assign assigned;
            if (rHit.collider.GetComponent<VIDE_Assign>() != null)
                assigned = rHit.collider.GetComponent<VIDE_Assign>();
            else return;

            if (assigned.alias == "QuestUI")
            {
                //questUI.Interact(); //Begins interaction with Quest Chart
            }
            else
            {
                diagUI.Interact(assigned); //Begins interaction
            }
        }
        */
    }
}
