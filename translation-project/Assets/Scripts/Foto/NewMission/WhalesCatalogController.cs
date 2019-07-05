using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhalesCatalogController : MonoBehaviour {

    public GameObject panelContent;
    public GameObject whaleCatalogPanel;

    public void Start()
    {
                
    }

    // not used
    public void CadastrarNovaBaleia()
    {
        panelContent.SetActive(true);
        whaleCatalogPanel.SetActive(false);
        Parameters.ISWHALEIDENTIFIED = true;
    }

    private void OnEnable()
    {
        if (Parameters.HIGH_CONTRAST) HighContrastText.ChangeTextBackgroundColor();
    }
}