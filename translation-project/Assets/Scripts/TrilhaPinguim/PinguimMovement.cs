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

        CheckEndGame(gameObject.name, collision);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Object: " + gameObject.name + " Collided in: " + collision.collider);

        if (collision.collider.ToString().Contains("pinguim"))
            pinguimController.CountTime(0.05f);
    }

    private void CheckEndGame(string pinguim, Collider2D peixe)
    {
        if (pinguim.Equals("pinguim_adelia") && peixe.name.Equals("peixe_adelia"))
        {
            pinguimController.adeliaFinished = true;
            pinguimController.pinguim_adelia.SetActive(false);
            peixe.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(0.4575472f, 1f, 0.4743936f);
        }

        if (pinguim.Equals("pinguim_antartico") && peixe.name.Equals("peixe_antartico"))
        {
            pinguimController.antarticoFinished = true;
            pinguimController.pinguim_antartico.SetActive(false);
            peixe.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(0.4575472f, 1f, 0.4743936f);
        }

        if (pinguim.Equals("pinguim_papua") && peixe.name.Equals("peixe_papua"))
        {
            pinguimController.papuaFinished = true;
            pinguimController.pinguim_papua.SetActive(false);
            peixe.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(0.4575472f, 1f, 0.4743936f);
        }

        if(peixe.name.Contains("peixe"))
            SelectNextPinguim();
    }

    private void SelectNextPinguim()
    {
        if (pinguimController.adeliaFinished)
        {
            if (pinguimController.antarticoFinished)
                pinguimController.SelectPinguin("papua");
            else
                pinguimController.SelectPinguin("antartico");
        }
        else if(pinguimController.antarticoFinished)
        {
            if (pinguimController.adeliaFinished)
                pinguimController.SelectPinguin("papua");
            else
                pinguimController.SelectPinguin("adelia");
        }
        else if(pinguimController.papuaFinished)
        {
            if (pinguimController.antarticoFinished)
                pinguimController.SelectPinguin("adelia");
            else
                pinguimController.SelectPinguin("antartico");
        }
    }
}
