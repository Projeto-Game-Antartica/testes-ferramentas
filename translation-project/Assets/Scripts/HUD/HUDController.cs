using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDController : AbstractScreenReader {

    private const float time = 0.03f;

    public GameObject instructionInterface;

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
    public GameObject missionName;
    public GameObject missionDescription;
    private readonly int INFO = 2;

    //private string navioDescription = "Notas proemias Convés de navio: " +
    //    "Convés de navio, com piso listrado marrom, do lado direito popa de três andares, branca com portas marrons, e do lado esquerdo popa com porta." +
    //    "Personagem principal no canto superior esquerdo da tela com duas barras uma em cima da outra ao seu lado direito. A barra de cima vermelha, representada por " +
    //    "um coração vermelho é a carga vida do personagem.A debaixo, amarela, " +
    //    "representada por uma estrela amarela, representa os pontos de experiência do personagem." +
    //    "Livro marrom com fita vermelha no canto inferior direito que represent o logbook do personagem, ainda não está em funcionamento." +
    //    "Mochila branca e marrom no canto inferior esquerdo que representa o inventário do personagem com os itens já adquiridos.";

    private void Start()
    {
        // "InstructionInterface" set on the main menu script
        if (PlayerPrefs.GetInt("InstructionInterface", 0) <= 0) 
            ActivateInstructionInterface();

        instructionInterface.GetComponentInChildren<Button>().Select();
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
        if(Input.GetKeyDown(InputKeys.QUEST_KEY))
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
            StartCoroutine(FillImage(bar, BAG));
        else if(bar.fillAmount == 1)
            StartCoroutine(UnfillImage(bar, BAG));
    }

    public void HandleInfoBar()
    {
        if (info.fillAmount == 0)
            StartCoroutine(FillImage(info, INFO));
        else if (info.fillAmount == 1)
            StartCoroutine(UnfillImage(info, INFO));
    }

    public IEnumerator FillImage(Image img, int op)
    {
        while (img.fillAmount < 1)
        {
            Debug.Log("openning");
            img.fillAmount += 0.1f;

            if ((img.fillAmount > 0.4f && img.fillAmount < 0.6f) && op == BAG) // its float numbers
                StartCoroutine(ShowInvItems());

            yield return new WaitForSeconds(time);
        }

        if (op == BAG) ponteiraButton.fillAmount = 1;
        else if (op == INFO)
        {
            missionDescription.SetActive(true);
            missionName.SetActive(true);
        }
    }

    public IEnumerator UnfillImage(Image img, int op)
    {
        if (op == BAG) ponteiraButton.fillAmount = 0;
        else if (op == INFO)
        {
            missionDescription.SetActive(false);
            missionName.SetActive(false);
        }

        while (img.fillAmount > 0)
        {
            Debug.Log("closing");

            img.fillAmount -= 0.1f;

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
        ReadText(ReadableTexts.key_navio_instructions);
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
