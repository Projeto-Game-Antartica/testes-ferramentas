using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using VIDE_Data;

public class HUDController : AbstractScreenReader {

    private const float time = 0.03f;
    
    // info settings
    public Image info;
    public GameObject infoMissionName;
    public GameObject infoMissionDescription;
    public GameObject infoMissionIcon;
    private readonly int INFO = 2;

    public GameObject inGameOption;

    // instruction interface settings
    public TextMeshProUGUI playerName;
    public GameObject instructionInterface;
    public TextMeshProUGUI missionTitle;
    public TextMeshProUGUI missionDescription;
    public Button iniciarButton;
    public Button fecharButton;

    public SimpleCharacterController simpleCharacterController;

    public AudioSource audioSource;

    public AudioClip closeClip;
    public AudioClip openBagClip;
    public AudioClip closeBagClip;

    public GameObject map;
    public TextMeshProUGUI mapText;

    public GameObject acessoTeclado;
    public TextMeshProUGUI descricaoText;

    public GameObject placaHUD;

    public BagController bagController;

    public LifeExpController lifeExpController;

    public string missionNumber;
    public string missionAudiodescriptionKey;

    private void Start()
    {
        playerName.text = PlayerPreferences.PlayerName.ToUpper();

        ReadInstructionAudiodescrition(missionNumber);

        Debug.Log(PlayerPrefs.GetInt("InstructionInterface", 0));

        // "InstructionInterface" set on the main menu script
        if (PlayerPrefs.GetInt("InstructionInterface", 0) <= 0)
        {
            ActivateInstructionInterface();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            if (instructionInterface.activeSelf)
                ReadInstructionAudiodescrition(missionNumber);
            else if (!instructionInterface.activeSelf && !inGameOption.activeSelf)
                ReadSceneAudiodescription(missionNumber);
            else if (inGameOption.activeSelf)
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_ingamemenu, LocalizationManager.instance.GetLozalization()));
        }


        if (Input.GetKeyDown(InputKeys.INSTRUCTIONS_KEY))
        {
            if (!instructionInterface.activeSelf)
            {
                ReadText("Menu de instruções aberto");
                ActivateInstructionInterface();
            }
            else
            {
                ActivateInstructionInterface();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!VD.isActive)
            {
                if(map.activeSelf)
                {
                    ShowMap(false);
                }
                else if (acessoTeclado.activeSelf)
                {
                    acessoTeclado.SetActive(false);
                }
                else if (placaHUD.activeSelf)
                {
                    placaHUD.SetActive(false);
                    ReadText("Placa fechada");
                }
                else if (!inGameOption.activeSelf && !instructionInterface.activeSelf)
                {
                    inGameOption.SetActive(true);
                    ReadText("Menu de opções aberto");
                    ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_ingamemenu, LocalizationManager.instance.GetLozalization()));
                }
                else
                {
                    inGameOption.SetActive(false);
                    ReadText("Menu de opções fechado");
                    audioSource.PlayOneShot(closeClip);
                }

                if (instructionInterface.activeSelf)
                {
                    instructionInterface.SetActive(false);
                    inGameOption.SetActive(false);
                    ReadText("Menu de instruções fechado");
                    audioSource.PlayOneShot(closeClip);
                }

                
            }
            else
            {
                //Debug.Log("SimpleCharacter");
                simpleCharacterController.diagUI.EndDialogue(VD.nodeData);
                inGameOption.SetActive(false);
            }
        }

        if (Input.GetKeyDown(InputKeys.QUEST_KEY))
        {
            HandleInfoBar();
        }

        if(Input.GetKeyDown(InputKeys.INVENTORY_KEY))
        {
            bagController.OpenOrClose();
        }

        if (Input.GetKeyDown(InputKeys.ACESSOTECLADO_KEY))
        {
            acessoTeclado.SetActive(true);
            ReadText("Navegação via teclado");
            ReadText(descricaoText.text);
            acessoTeclado.GetComponentInChildren<Button>().Select();
        }

        if (Input.GetKeyDown(InputKeys.MAP_KEY))
        {
            if (map.activeSelf)
            {
                ShowMap(false);
            }
            else
            {
                ShowMap(true);
            }
        }

        if (Input.GetKeyDown(InputKeys.PARAMETERS_KEY))
        {
            lifeExpController.ReadHPandEXP();
        }
    }

    public void InitializeGame()
    {
        instructionInterface.SetActive(false);

        ShowMap(true);

        ReadText(ReadableTexts.instance.GetReadableText(missionAudiodescriptionKey, LocalizationManager.instance.GetLozalization()));

        iniciarButton.gameObject.SetActive(false);
        fecharButton.gameObject.SetActive(true);

        iniciarButton.Select();
    }

    public void ActivateInstructionInterface()
    {
        instructionInterface.SetActive(true);
        PlayerPrefs.SetInt("InstructionInterface", 1);

        ReadText(missionTitle.text);
        ReadText(missionDescription.text);

        Debug.Log(missionTitle.text);
        Debug.Log(missionDescription.text);

        if (iniciarButton.isActiveAndEnabled)
            iniciarButton.Select();
        else
            fecharButton.Select();
    }

    void ReadInstructionAudiodescrition(string missionNumber)
    {
        switch(missionNumber)
        {
            case "M002":
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m002_instrucoes, LocalizationManager.instance.GetLozalization()));
                break;
            case "M002_Casinha":
                break;
            case "M004":
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_navio_instrucao, LocalizationManager.instance.GetLozalization()));
                break;
            case "M009":
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m009_missao_instrucao, LocalizationManager.instance.GetLozalization()));
                break;
            default:
                Debug.Log("check mission number...");
                break;
        }

        ReadText(missionTitle.text);
        ReadText(missionDescription.text);
    }

    public void ShowMap(bool op)
    {
        if(op)
        {
            map.SetActive(true);

            ReadText("mapa aberto.");

            switch (missionNumber)
            {
                case "M002":
                    if (PlayerPrefs.GetInt("UshuaiaMap", 0) <= 0)
                    {
                        ReadText(mapText.text);
                        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m002_itens_mapa, LocalizationManager.instance.GetLozalization()));
                    }
                    break;
                case "M002_Casinha":
                    if (PlayerPrefs.GetInt("CasaUshuaiaMap", 0) <= 0)
                    {
                        ReadText(mapText.text);
                        ReadText("");
                    }
                    break;
                case "M004":
                    if (PlayerPrefs.GetInt("NavioMap", 0) <= 0)
                    {
                        ReadText(mapText.text);
                        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_navio_mapa, LocalizationManager.instance.GetLozalization()));
                
                    }
                    break;

                case "M009":
                    if (PlayerPrefs.GetInt("CampMap", 0) <= 0)
                    {
                        ReadText(mapText.text);
                        ReadText(ReadableTexts.instance.GetReadableText("m009_missao", LocalizationManager.instance.GetLozalization()));
                    }
                    break;

                case "M010":
                    if (PlayerPrefs.GetInt("VegMap", 0) <= 0)
                    {
                        ReadText(mapText.text);
                        ReadText(ReadableTexts.instance.GetReadableText("m010_full_scene", LocalizationManager.instance.GetLozalization()));
                
                    }
                    break;
            }
        }
        else
        {
            map.SetActive(false);

            ReadText("mapa fechado.");

            switch (missionNumber)
            {
                case "M002":
                    PlayerPrefs.SetInt("UshuaiaMap", 1);
                    break;
                case "M002_Casinha":
                    PlayerPrefs.SetInt("CasaUshuaiaMap", 1);
                    break;
                case "M004":
                    PlayerPrefs.SetInt("NavioMap", 1);
                    break;
                case "M009":
                    PlayerPrefs.SetInt("CampMap", 1);
                    break;
                case "M010":
                    PlayerPrefs.SetInt("VegMap", 1);
                    break;
            }
        }
    }

    public void HandleInfoBar()
    {
        if (info.fillAmount == 0)
        {
            StartCoroutine(FillImage(info, INFO));
            ReadText("Informações ativadas");
            ReadText(infoMissionName.GetComponentInChildren<TextMeshProUGUI>().text);
            ReadText(infoMissionDescription.GetComponentInChildren<TextMeshProUGUI>().text);
        }
        else if (info.fillAmount == 1)
        {
            StartCoroutine(UnfillImage(info, INFO));
            ReadText("Informações desativadas");
        }
    }

    public IEnumerator FillImage(Image img, int op)
    {
        while (img.fillAmount < 1)
        {
            Debug.Log("openning");
            img.fillAmount += 0.05f;

            yield return new WaitForSeconds(time);
        }
        if (op == INFO)
        {
            infoMissionDescription.SetActive(true);
            infoMissionName.SetActive(true);
            infoMissionIcon.SetActive(true);
        }
    }

    public IEnumerator UnfillImage(Image img, int op)
    {
        //if (op == BAG) ponteiraButton.fillAmount = 0;
        if (op == INFO)
        {
            infoMissionDescription.SetActive(false);
            infoMissionName.SetActive(false);
            infoMissionIcon.SetActive(false);
        }

        while (img.fillAmount > 0)
        {
            Debug.Log("closing");

            img.fillAmount -= 0.05f;

            yield return new WaitForSeconds(time);
        }
    }

    public void ReadSceneAudiodescription(string missionNumber)
    {
        switch (missionNumber)
        {
            case "M002":
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m002_itens_mapa, LocalizationManager.instance.GetLozalization()));
                break;
            case "M002_Casinha":
                break;
            case "M004":
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_navio, LocalizationManager.instance.GetLozalization()));
                break;
            case "M009":
                ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m009_missao, LocalizationManager.instance.GetLozalization()));
                break;
            default:
                Debug.Log("check mission number...");
                break;
        }

    }

    public void ChangeMission(TextMeshProUGUI missionName)
    {
        // not save any position before changing scene
        PlayerPrefs.SetInt("Saved", 0);

        // to show instruction interface when changing scene
        PlayerPrefs.SetInt("InstructionInterface", 0);

        switch (missionName.text.ToLower())
        {
            case "baleias":
                SceneManager.LoadScene(ScenesNames.M004Ship);
                break;
            case "itens":
                SceneManager.LoadScene(ScenesNames.M002Ushuaia);
                break;
            case "paleontologia":
                SceneManager.LoadScene(ScenesNames.M009Camp);
                break;
            case "vegetação":
                SceneManager.LoadScene(ScenesNames.M010Camp);
                break;

        }
    }
}
