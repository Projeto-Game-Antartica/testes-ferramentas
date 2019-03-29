using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CatalogController : MonoBehaviour {

    public WhaleController whaleController;
    public GameObject panelCatalog;
    public Image scrollSnapImage;
    public Image contornoBaleiaPrincipal;

    public void CheckWhale()
    {
        string whale_image = "Whales/" + GetComponent<Image>().sprite.name;

        Debug.Log(whale_image);
        // id of whale on the photo
        int whale_id = CameraOverlayController.randomID;

        if (whale_image.Equals(whaleController.getWhaleById(whale_id).image_path)) 
        {
            scrollSnapImage.color = Color.green;
            //panelCatalog.SetActive(false);
        }
        else
        {
            scrollSnapImage.color = Color.red;
        }
    }

    public void ResetScrollSnapColor()
    {
        scrollSnapImage.color = Color.white;
    }

    public void ResetContornoPrincipalColor()
    {
        contornoBaleiaPrincipal.color = Color.black;
    }

    public void setContornoBaleiaPrincipal()
    {
        contornoBaleiaPrincipal.color = Color.yellow;
    }
}
