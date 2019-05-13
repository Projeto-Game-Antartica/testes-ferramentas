using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// not used
public class ContentPanelController : AbstractScreenReader {

    public Button first_button;
    public GameObject panelCatalogo;
    public GameObject panelNomear;
    public GameObject panelOptions;
    public GameObject panelFotoidentificacao;
    public GameObject titleText;
    public Button saveButton;
    public InputField whaleNameInput;
    public WhaleController whaleController;
    public GameObject[] buttons;
    public FotoidentificacaoController fotoidentificacaoController;

    private ReadableTexts readableTexts;
    
    private void Start()
    {
        //TolkUtil.Load();
        //Parameters.ACCESSIBILITY = true;
        
        readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        ReadInstructions();
        first_button.Select();
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReadInstructions();
        }

        // not the best way
        if (Parameters.ISWHALEIDENTIFIED)
        {
            ActivatePanelNomear();
        }
        else
        {
            if (panelNomear != null) panelNomear.SetActive(false);
        }
	}
    
    private void OnEnable()
    {
        if (Parameters.HIGH_CONTRAST) HighContrastText.ChangeTextBackgroundColor();
    }

    public void ReadInstructions()
    {
        ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_catalogDescription, LocalizationManager.instance.GetLozalization()));
    }

    public void Save()
    {
        gameObject.SetActive(false);
        panelCatalogo.SetActive(true);
    }

    public void StartFotoidenfiticacao(TextMeshProUGUI type)
    {
        gameObject.SetActive(false);
        panelFotoidentificacao.SetActive(true);
        
        switch(type.text.ToLower())
        {
            case "pigmentação":
                fotoidentificacaoController.RoundSettings(Parameters.PIGMENTACAO);
                break;
            case "manchas":
                fotoidentificacaoController.RoundSettings(Parameters.MANCHAS);
                break;
            case "riscos":
                fotoidentificacaoController.RoundSettings(Parameters.RISCOS);
                break;
            case "marcas":
                fotoidentificacaoController.RoundSettings(Parameters.MARCAS);
                break;
            case "borda":
                fotoidentificacaoController.RoundSettings(Parameters.BORDA);
                break;
            case "ponta":
                fotoidentificacaoController.RoundSettings(Parameters.PONTAS);
                break;
            case "entalhe":
                fotoidentificacaoController.RoundSettings(Parameters.ENTALHE);
                break;
            default:
                break;
        }
    }

    public void ActivatePanelNomear()
    {
        if (panelNomear != null)
        {
            panelOptions.SetActive(false);
            titleText.SetActive(false);
            saveButton.enabled = false;
            panelNomear.SetActive(true);

            string whale_name = whaleController.getWhaleById(Parameters.WHALE_ID).whale_name;

            if (!whale_name.Equals(""))
            {
                Debug.Log(whale_name);
                whaleNameInput.text = whale_name;
                //whaleNameInput.text = whale_name;
                whaleNameInput.interactable = false;
            }
            else
            {
                whaleNameInput.interactable = true;
                whaleNameInput.Select();
            }
        }
    }

    public void ClearInputFieldPanelNomear()
    {
        whaleNameInput.text = string.Empty;
    }

    public void ResetButtonColors()
    {
        foreach(GameObject b in buttons)
        {
            b.GetComponent<Image>().color = Color.white;
        }
    }
}
