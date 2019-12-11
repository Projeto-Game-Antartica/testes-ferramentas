using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using VIDE_Data;

public class HUDController : AbstractScreenReader {

    private const float time = 0.03f;
    
    // bag bar settings
    public Image bar;
    public Image camera_inv;
    public Image lente_inv;
    public Image ponteiraButton;
    private readonly int BAG = 1;

    public Sprite camera_cinza;
    public Sprite camera_color;
    public Sprite lente_cinza;
    public Sprite lente_color;

    // info settings
    public Image info;
    public GameObject infoMissionName;
    public GameObject infoMissionDescription;
    private readonly int INFO = 2;

    public GameObject inGameOption;

    // instruction interface settings
    public GameObject instructionInterface;
    public TextMeshProUGUI missionTitle;
    public TextMeshProUGUI missionDescription;
    public Button iniciarButton;

    public SimpleCharacterController simpleCharacterController;

    public AudioSource audioSource;

    public AudioClip closeClip;
    public AudioClip openBagClip;
    public AudioClip closeBagClip;

    private void Start()
    {
        // "InstructionInterface" set on the main menu script
        if (PlayerPrefs.GetInt("InstructionInterface", 0) <= 0)
        {
            ActivateInstructionInterface();
            ReadInstructions();
        }

        iniciarButton.Select();
    }

    private void LateUpdate()
    {
        if (!PlayerPreferences.M004_Memoria)
            camera_inv.sprite = camera_cinza;
        else
            camera_inv.sprite = camera_color;

        if (!PlayerPreferences.M004_TeiaAlimentar)
            lente_inv.sprite = lente_cinza;
        else
            lente_inv.sprite = lente_color;
           
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputKeys.INSTRUCTIONS_KEY))
        {
            if (!instructionInterface.activeSelf)
            {
                ReadText("Menu de instruções aberto");
                instructionInterface.SetActive(true);
                ReadInstructions();
            }
            else
            {
                ReadInstructions();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!VD.isActive)
            {
                Debug.Log("HUDController");
                if (!inGameOption.activeSelf && !instructionInterface.activeSelf)
                {
                    ReadText("Menu de opções aberto");
                    inGameOption.SetActive(true);
                    inGameOption.GetComponentInChildren<Button>().Select();
                }
                else
                {
                    //ReadText("Menu de opções fechado");
                    audioSource.PlayOneShot(closeClip);
                    inGameOption.SetActive(false);
                }

                if (instructionInterface.activeSelf)
                {
                    //ReadText("Menu de instruções fechado");
                    audioSource.PlayOneShot(closeClip);
                    instructionInterface.SetActive(false);
                    inGameOption.SetActive(false);
                }
            }
            else
            {
                Debug.Log("SimpleCharacter");
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
            HandleBagBar();
        }
    }

    public void ActivateInstructionInterface()
    {
        instructionInterface.SetActive(true);
        PlayerPrefs.SetInt("InstructionInterface", 1);
    }

    public void HandleBagBar()
    {
        //StartCoroutine(HandleBagBar());

        if (bar.fillAmount == 0)
        {
            StartCoroutine(FillImage(bar, BAG));
            audioSource.PlayOneShot(openBagClip);
        }
        else if (bar.fillAmount == 1)
        {
            StartCoroutine(UnfillImage(bar, BAG));
            audioSource.PlayOneShot(closeBagClip);
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

            if ((img.fillAmount > 0.4f && img.fillAmount < 0.6f) && op == BAG) // its float numbers
                StartCoroutine(ShowInvItems());

            yield return new WaitForSeconds(time);
        }

        if (op == BAG) ponteiraButton.fillAmount = 1;
        else if (op == INFO)
        {
            infoMissionDescription.SetActive(true);
            infoMissionName.SetActive(true);
        }
    }

    public IEnumerator UnfillImage(Image img, int op)
    {
        if (op == BAG) ponteiraButton.fillAmount = 0;
        else if (op == INFO)
        {
            infoMissionDescription.SetActive(false);
            infoMissionName.SetActive(false);
        }

        while (img.fillAmount > 0)
        {
            Debug.Log("closing");

            img.fillAmount -= 0.05f;

            if ((img.fillAmount > 0.5f && img.fillAmount < 0.7f) && op == BAG) // its float numbers
                StartCoroutine(CoverInvItems());

            yield return new WaitForSeconds(time);
        }
    } 

    public IEnumerator ShowInvItems()
    {
        while (camera_inv.fillAmount < 1)
        {
            camera_inv.fillAmount += 0.1f;
            lente_inv.fillAmount += 0.1f;

            yield return new WaitForSeconds(time);
        }
    }

    public IEnumerator CoverInvItems()
    {
        while (camera_inv.fillAmount > 0)
        {
            camera_inv.fillAmount -= 0.1f;
            lente_inv.fillAmount -= 0.1f;

            yield return new WaitForSeconds(time);
        }
    }

    public void ReadInstructions()
    {
        ReadText(missionTitle.text);
        ReadText(missionDescription.text);
        Debug.Log(missionTitle.text);
        Debug.Log(missionDescription.text);
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
        }
    }
}
