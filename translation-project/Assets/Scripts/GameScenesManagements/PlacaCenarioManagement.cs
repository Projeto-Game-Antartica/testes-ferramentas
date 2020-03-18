using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaCenarioManagement : AbstractScreenReader {

    public GameObject placaHUD;
    public AudioSource audioSource;

    void Start()
    {
        audioSource.loop = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if (collision.name.Equals("Turista"))
        {
            audioSource.Play();
            
            placaHUD.SetActive(true);
            Debug.Log("placa aberta");
            ReadText("placa aberta");

            placaHUD.GetComponentInChildren<UnityEngine.UI.Button>().Select();
        }
    }

    //public void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.name.Equals("Turista"))
    //    {
    //        placaHUD.SetActive(true);
    //    }
    //}

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Equals("Turista"))
        {
            audioSource.Stop();
            placaHUD.SetActive(false);
            Debug.Log("placa fechada");
            ReadText("placa fechada");
        }
    }
}
