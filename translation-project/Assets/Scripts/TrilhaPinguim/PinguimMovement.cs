using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PinguimMovement : MonoBehaviour {

    public PinguimController pinguimController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Object: " + gameObject.name + " Hitted: " + collision.name);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Object: " + gameObject.name + " Collided in: " + collision.collider);

        if (collision.collider.ToString().Contains("pinguim"))
            pinguimController.LoseHP();
    }
}
