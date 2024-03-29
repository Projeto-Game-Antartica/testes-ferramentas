﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingCamera : MonoBehaviour {

    //Public variable to store a reference to the player game object
    public GameObject player;

    //Private variable to store the offset distance between the player and camera
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
    }
    
    // set only the x and y component
    public void SetCameraPosition(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);

        Debug.Log(transform.position);
    }
}
