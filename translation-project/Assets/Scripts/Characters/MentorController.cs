using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentorController : MonoBehaviour {

    public SpriteRenderer dialogPanel;


    private void OnTriggerStay(Collider other)
    {
        dialogPanel.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        dialogPanel.enabled = false;
    }

}
