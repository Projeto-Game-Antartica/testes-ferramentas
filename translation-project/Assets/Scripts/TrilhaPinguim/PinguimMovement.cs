using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PinguimMovement : AbstractScreenReader {

    public PinguimController pinguimController;

    public AudioSource audioSource;

    public AudioClip pinguimFonteClip;
    public AudioClip bloqueioClip;
    public AudioClip mapLimit;

    public Image adeliaIcon;
    public Image antarticoIcon;
    public Image papuaIcon;

    private int pinguimPosition = 0;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            ReadPinguimPosition("adelia");
            ReadPinguimPosition("antartico");
            ReadPinguimPosition("papua");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Object: " + gameObject.name + " Triggered: " + collision.name);

        // verify if collision.name is a integer
        int.TryParse(collision.name, out pinguimPosition);

        CheckEndGame(gameObject.name, collision, pinguimController.isMapping());
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Object: " + gameObject.name + " Collided in: " + collision.collider);
        
        if (collision.collider.ToString().Contains("pinguim"))
        {
            pinguimController.timerCount += 10f;
            audioSource.PlayOneShot(bloqueioClip);
        }
        else
        {
            audioSource.PlayOneShot(mapLimit);
        }
    }

    private void CheckEndGame(string pinguim, Collider2D peixe, bool isMapping)
    {
        // check end game only if its not mapping the trail
        if (!isMapping)
        {
            if (pinguim.Equals("pinguim_adelia") && peixe.name.Equals("peixe_adelia"))
            {
                pinguimController.adeliaFinished = true;
                pinguimController.adeliaButton.interactable = false;
                pinguimController.pinguim_adelia.SetActive(false);
                adeliaIcon.color = new Color(0.4575472f, 1f, 0.4743936f);
                audioSource.PlayOneShot(pinguimFonteClip);
            }

            if (pinguim.Equals("pinguim_antartico") && peixe.name.Equals("peixe_antartico"))
            {
                pinguimController.antarticoFinished = true;
                pinguimController.antarticoButton.interactable = false;
                pinguimController.pinguim_antartico.SetActive(false);
                antarticoIcon.color = new Color(0.4575472f, 1f, 0.4743936f);
                audioSource.PlayOneShot(pinguimFonteClip);
            }

            if (pinguim.Equals("pinguim_papua") && peixe.name.Equals("peixe_papua"))
            {
                pinguimController.papuaFinished = true;
                pinguimController.papuaButton.interactable = false;
                pinguimController.pinguim_papua.SetActive(false);
                papuaIcon.color = new Color(0.4575472f, 1f, 0.4743936f);
                audioSource.PlayOneShot(pinguimFonteClip);
            }

            if (peixe.name.Contains("peixe"))
                SelectNextPinguim();
        }
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

    public void ReadPinguimPosition(string pinguimName)
    {
        //string pinguimName = gameObject.name.Replace("_", " ");

        Debug.Log("O " + pinguimName + " está na posição " + pinguimPosition);
        ReadText("O " + pinguimName + " está na posição " + pinguimPosition);
    }
}
