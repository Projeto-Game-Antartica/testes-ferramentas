using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCatalogController : MonoBehaviour {

    [SerializeField]
    private WhaleController whaleController;
    [SerializeField]
    private GameObject panelWhaleCatalog;
    [SerializeField]    
    private Image contornoBaleiaPrincipal;
    [SerializeField]   
    private Image whaleSprite;
    [SerializeField]
    private GameObject panelContent;
    [SerializeField]
    private InputField inputField;

    public GameObject nova_baleia;
    public Text description;
    public GameObject[] buttons;

    public void CheckWhale()
    {
        // NEW MISSION
        //string whale_image = "Whales/" + gameObject.GetComponentInChildren<Image>().sprite.name;
        string whale_image = "Whales/" + whaleSprite.sprite.name;

        Debug.Log(whale_image);

        // id of whale on the photo
        int whale_id = Parameters.WHALE_ID;

        Debug.Log(whaleController.getWhaleById(whale_id).image_path);

        if (whale_image.Equals(whaleController.getWhaleById(whale_id).image_path))
        {
            //GetComponent<Image>().color = Color.green;
            panelWhaleCatalog.SetActive(false);
            panelContent.SetActive(true);
            Parameters.ISWHALEIDENTIFIED = true;
        }
        else
        {
            GetComponent<Image>().color = Color.red;
        }

        // OLD MISSION
        //string whale_image = "Whales/" + gameObject.GetComponentInChildren<Image>().sprite.name;
        //string whale_image = "Whales/" + whaleSprite.sprite.name;

        //Debug.Log(whale_image);

        //// id of whale on the photo
        //int whale_id = Parameters.WHALE_ID;

        //Debug.Log(whaleController.getWhaleById(whale_id).image_path);

        //if (whale_image.Equals(whaleController.getWhaleById(whale_id).image_path))
        //{
        //    description.gameObject.SetActive(false);
        //    nova_baleia.SetActive(true);
        //    inputField.text = whaleController.getWhaleById(Parameters.WHALE_ID).whale_name;
        //    inputField.interactable = false;
        //}
        //else
        //{
        //    GetComponent<Image>().color = Color.red;
        //}
    }

    public void ResetScrollSnapColor()
    {
        GetComponent<Image>().color = Color.white;
    }

    public void ResetContornoPrincipalColor()
    {
        contornoBaleiaPrincipal.color = Color.black;
    }

    public void setContornoBaleiaPrincipal()
    {
        contornoBaleiaPrincipal.color = Color.yellow;

        inputField.text = string.Empty;
        inputField.interactable = true;
    }

    public void ResetAllContornoColor()
    {
        foreach (GameObject b in buttons)
        {
            b.GetComponent<Image>().color = Color.white;
        }
    }
}
