using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

// not used
public class ContentPanelMissionController : AbstractScreenReader {
    
    public Button saveButton;
    public TMP_InputField whaleNameInput;
    public WhaleController whaleController;
    public GameObject warningInterface;

    public TextMeshProUGUI whaleCountText;
    public TextMeshProUGUI dateText;

    public GameObject WinImage;
    public AudioClip victoryClip;
    public AudioClip avisoClip;
    public AudioClip closeClip;
    public AudioSource audioSource;

    public TailMissionSceneManager tailMissionSceneManager;

    public ButtonCatalogMissionController buttonCatalogMission;

    WhaleData whale;

    private int count = 0;

    private int selectedArea = 0;

    public LifeExpController lifeExpController;

    public static bool isOnInputfield = false;
    
    private void Start()
    {
        whaleCountText.text = count.ToString();
        //Parameters.ACCESSIBILITY = true;

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_desafio_catalogo, LocalizationManager.instance.GetLozalization()));

        //saveButton.Select();
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.F6))
        {
            selectedArea = (selectedArea + 1) % 3;

            if (selectedArea == 1)
            {
                buttonCatalogMission.buttons[0].GetComponent<Button>().Select();
            }
            else if (selectedArea == 2)
            {
                // audiodescricao da baleia e suas info
                ReadWhaleInfo(whale);
            }
            else
            {
                whaleNameInput.Select();
            }
        }

        if (Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_desafio_catalogo, LocalizationManager.instance.GetLozalization()));
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(saveButton.interactable)
                Save();
        }

        if(EventSystem.current.currentSelectedGameObject == whaleNameInput.gameObject)
        {
            isOnInputfield = true;
        }
        else
        {
            isOnInputfield = false;
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
            whale = whaleData;

            string result = "Características da cauda fotografada:" +  whaleController.getWhaleById(whaleData.id_whale).description;

            result += "Informações referente à baleia fotografada: " + "Data da foto " + dateText.text + " Organização ou Operador: INTERANTAR "
                + " A Localização é: Latitude: " + whaleData.latitude + " Longitude: " + whaleData.longitude;

            ReadText(result);
            Debug.Log(result);
        }
        catch (Exception ex)
        {
            Debug.Log("Baleia não encontrada. Stacktrace >> " + ex.StackTrace);
        }

    }

    public void Save()
    {
        //whaleController.getWhaleById(Parameters.WHALE_ID).indentified = true;

        if (string.IsNullOrEmpty(whaleNameInput.text))
        {
            Debug.Log("O nome não pode ser vazio!");
            ReadText("O nome não pode ser vazio!");

            ReadText("Insira um nome para a baleia fotografada");

            whaleNameInput.Select();
        }
        else
        {
            whaleController.getWhaleById(Parameters.WHALE_ID).whale_name = whaleNameInput.text;

            audioSource.PlayOneShot(avisoClip);

            warningInterface.SetActive(true);

            count++;

            whaleCountText.text = count.ToString();

            if (count < 4)
            {
                if (!whaleController.getWhaleById(Parameters.WHALE_ID).whale_name.Equals(""))
                    warningInterface.GetComponentInChildren<TextMeshProUGUI>().text = "Parabéns, baleia identificada. Realize uma nova foto.";
                else
                    warningInterface.GetComponentInChildren<TextMeshProUGUI>().text = "Parabéns, baleia cadastrada. Realize uma nova foto.";

                ReadText("Nome da baleia: " + whaleNameInput.text);
                ReadText(warningInterface.GetComponentInChildren<TextMeshProUGUI>().text);

                //StartCoroutine(BackToPhotoCoroutine());
            }
            //else 
            //{
            //    StartCoroutine(FinishMissionCoroutine());
            //}
            //gameObject.SetActive(false);

            warningInterface.GetComponentInChildren<Button>().Select();
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
        yield return new WaitForSeconds(4);

        warningInterface.SetActive(false);
        gameObject.SetActive(false);
    }

    public IEnumerator FinishMissionCoroutine()
    {
        WinImage.SetActive(true);

        lifeExpController.AddEXP(PlayerPreferences.XPwinMission); // finalizou a missão
        lifeExpController.AddHP(PlayerPreferences.HPwinMission); 

        audioSource.PlayOneShot(victoryClip);

        ReadText("Parabéns, você concluiu esta missão e colaborou com a Ciência Cidadã. Siga agora para mais um desafio, escolhendo uma nova missão.");
        
        yield return new WaitWhile(() => audioSource.isPlaying);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_desafio_sucesso, LocalizationManager.instance.GetLozalization()));

        yield return new WaitForSeconds(7f);

        tailMissionSceneManager.ReturnToShip();
    }
}
