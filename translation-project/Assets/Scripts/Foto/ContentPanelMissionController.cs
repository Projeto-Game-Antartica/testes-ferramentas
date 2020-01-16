using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// not used
public class ContentPanelMissionController : AbstractScreenReader {
    
    public Button saveButton;
    public TMP_InputField whaleNameInput;
    public WhaleController whaleController;
    public GameObject confirmFoto;

    private ReadableTexts readableTexts;

    public TextMeshProUGUI confirmText;
    public TextMeshProUGUI whaleCountText;
    public TextMeshProUGUI dateText;

    public GameObject WinImage;
    public AudioClip victoryClip;
    public AudioClip avisoClip;
    public AudioSource audioSource;

    public TailMissionSceneManager tailMissionSceneManager;

    public ButtonCatalogMissionController buttonCatalogMission;

    WhaleData whale;

    private int count = 0;

    private bool onWhaleCatalog = false;

    public LifeExpController lifeExpController;
    
    private void Start()
    {
        whaleCountText.text = count.ToString();
        //TolkUtil.Load();
        //Parameters.ACCESSIBILITY = true;
        
        //readableTexts = GameObject.Find("ReadableTexts").GetComponent<ReadableTexts>();
        ReadInstructions();
        saveButton.Select();
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReadInstructions();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            if(!onWhaleCatalog)
            {
                buttonCatalogMission.buttons[0].GetComponent<Button>().Select();
                onWhaleCatalog = true;
            }
            else
            {
                // audiodescricao da baleia e suas info
                ReadWhaleInfo(whale);
                onWhaleCatalog = false;
            }
        }

        // not the best way
        //if (Parameters.ISWHALEIDENTIFIED)
        //{
        //    CheckWhaleName();
        //}
	}
    
    private void OnEnable()
    {

        //Debug.Log(Parameters.WHALE_ID);
        //// se a foto foi tirada corretamente, faz a leitura
        //if(Parameters.WHALE_ID != -1)
        //{
        //    whale = whaleController.getWhaleById(Parameters.WHALE_ID);
        //    ReadWhaleInfo(whale);
        //}
    }

    public void ReadWhaleInfo(WhaleData whaleData)
    {
        try
        {
            //string audiodescricao = "";

            string result = "Informações referente à baleia fotografada: " + "Data da foto " + dateText.text + " Organização ou Operador: INTERANTAR "
                + " A Localização é: Latitude: " + whaleData.latitude + " Longitude: " + whaleData.longitude;

            ReadText(result);
            Debug.Log(result);
        }
        catch (Exception ex)
        {
            Debug.Log("Baleia não encontrada. Stacktrace >> " + ex.StackTrace);
        }

    }

    public void ReadInstructions()
    {
        ReadText(readableTexts.GetReadableText(ReadableTexts.key_foto_catalogDescription, LocalizationManager.instance.GetLozalization()));
    }

    public void Save()
    {
        //whaleController.getWhaleById(Parameters.WHALE_ID).indentified = true;

        if (string.IsNullOrEmpty(whaleNameInput.text))
        {
            Debug.Log("O nome não pode ser vazio!");
            whaleNameInput.Select();
        }
        else
        {
            whaleController.getWhaleById(Parameters.WHALE_ID).whale_name = whaleNameInput.text;

            audioSource.PlayOneShot(avisoClip);

            confirmFoto.SetActive(true);

            count++;

            whaleCountText.text = count.ToString();

            if (count < 4)
            {
                if (!whaleController.getWhaleById(Parameters.WHALE_ID).whale_name.Equals(""))
                    confirmText.text = "Parabéns, baleia identificada. Realize uma nova foto.";
                else
                    confirmText.text = "Parabéns, baleia cadastrada. Realize uma nova foto.";

                StartCoroutine(BackToPhotoCoroutine());
            }
            else 
            {
                StartCoroutine(FinishMissionCoroutine());
            }
            //gameObject.SetActive(false);
        }
    }

    public void CheckWhaleName()
    {
        string whale_name = whaleController.getWhaleById(Parameters.WHALE_ID).whale_name;

        Debug.Log(whale_name);

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
            saveButton.interactable = true;

        //whaleController.getWhaleById(Parameters.WHALE_ID).indentified = true;
    }

    public void ClearNameInputField()
    {
        whaleNameInput.text = string.Empty;
    }

    public IEnumerator BackToPhotoCoroutine()
    {
        yield return new WaitForSeconds(2);

        confirmFoto.SetActive(false);
        gameObject.SetActive(false);
    }

    public IEnumerator FinishMissionCoroutine()
    {
        WinImage.SetActive(true);

        lifeExpController.AddEXP(PlayerPreferences.XPwinMission); // finalizou a missão
        lifeExpController.AddHP(PlayerPreferences.HPwinMission); 

        audioSource.PlayOneShot(victoryClip);

        yield return new WaitWhile(() => audioSource.isPlaying);

        yield return new WaitForSeconds(4f);

        tailMissionSceneManager.ReturnToShip();
    }
}
