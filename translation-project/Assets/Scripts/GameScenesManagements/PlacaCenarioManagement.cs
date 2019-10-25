using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaCenarioManagement : MonoBehaviour {

    public GameObject placaHUD;

    void Start()
    {
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if (collision.name.Equals("Turista"))
            placaHUD.SetActive(true);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name.Equals("Turista"))
            placaHUD.SetActive(true);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Equals("Turista"))
            placaHUD.SetActive(false);
    }
}
