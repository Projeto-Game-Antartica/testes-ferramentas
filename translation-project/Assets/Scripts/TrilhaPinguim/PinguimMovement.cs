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

        CheckEndGame(gameObject.name, collision.name);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Object: " + gameObject.name + " Collided in: " + collision.collider);

        if (collision.collider.ToString().Contains("pinguim"))
            pinguimController.LoseHP();
    }

    private void CheckEndGame(string pinguim, string peixe)
    {
        if(pinguim.Equals("pinguim_adelia") && peixe.Equals("peixe_adelia"))
            PinguimController.adeliaFinished = true;

        if (pinguim.Equals("pinguim_antartico") && peixe.Equals("peixe_antartico"))
            PinguimController.antarticoFinished = true;

        if (pinguim.Equals("pinguim_papua") && peixe.Equals("peixe_papua"))
            PinguimController.papuaFinished = true;
    }
}
