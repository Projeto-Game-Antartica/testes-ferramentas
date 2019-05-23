using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeExpController : MonoBehaviour {

    
    public Transform healthbar;
    public Transform expbar;

	// Use this for initialization
	void Start () {
        //healthbar.localScale = new Vector3(0f, 1f, 1f);
        //expbar.localScale = new Vector3(0f, 1f, 1f);
	}
	
	// Update is called once per frame
    // just for debug
	void Update () {
	    
    }

    private void HandleHelthBar()
    {
        if (healthbar.localScale.x <= 0)
        {
            healthbar.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            healthbar.localScale -= new Vector3(.01f, 0f, 0f);
        }

        if (expbar.localScale.x >= 1)
        {
            expbar.localScale = new Vector3(0f, 1f, 1f);
        }
        else
        {
            expbar.localScale += new Vector3(.01f, 0f, 0f);
        }
    }
}
