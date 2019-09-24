using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCatalogMissionController : MonoBehaviour {

    [SerializeField]
    private WhaleController whaleController;
    [SerializeField]    
    private Image contornoBaleiaPrincipal;
    [SerializeField]   
    private Image whaleSprite;
    [SerializeField]
    private InputField inputField;

    public Button nova_baleia;
    public GameObject[] buttons;

    public Button saveButton;

    public void CheckWhale()
    {
        // OLD MISSION
        //string whale_image = "Whales/" + gameObject.GetComponentInChildren<Image>().sprite.name;
        string whale_image = "Whales/" + whaleSprite.sprite.name;

        Debug.Log(whale_image);

        // id of whale on the photo
        int whale_id = Parameters.WHALE_ID;

        //Debug.Log(whaleController.getWhaleById(whale_id).image_path);

        if (Parameters.WHALE_ID != -1 && whale_image.Equals(whaleController.getWhaleById(whale_id).image_path))
        {
            GetComponent<Image>().color = Color.green;
            nova_baleia.interactable = true;
            inputField.text = whaleController.getWhaleById(Parameters.WHALE_ID).whale_name;
            inputField.interactable = false;

            saveButton.interactable = true;
        }
        else
        {
            GetComponent<Image>().color = Color.red;
        }
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
