using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ErasScene : AbstractScreenReader {
    
    public GameObject LoseImage;
    public TMPro.TextMeshProUGUI LoseText;

    public LifeExpController lifeExpController;

    public Button audioButton;
    public Button resetButton;
    public Button backButton;

    private bool isOnMenu = false;

    private bool started = false;
    private bool finished = false;
    private bool paused = false;

    public GameObject instructionInterface;
    public GameObject confirmQuit;

    public MinijogosDicas dicas;

    public AudioSource audioSource;
    public AudioClip loseClip;
    public AudioClip closeClip;
    public AudioClip avisoClip;

    public ErasPaleoController erasPaleoController;

    public void InitializeGame()
    {
        StartCoroutine(StartTimer());
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(0.5f);

        started = true;

        resetButton.interactable = true;
        backButton.interactable = true;

        erasPaleoController.SelectFirstItem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputKeys.INSTRUCTIONS_KEY))
        {
                instructionInterface.SetActive(true);
        }

        if(Input.GetKeyDown(InputKeys.PARAMETERS_KEY))
        {
            ReadMJMenu();
        }

        if (Input.GetKeyDown(InputKeys.MJMENU_KEY))
        {
            if (!isOnMenu)
            {
                audioButton.Select();
                isOnMenu = true;
            }
            else
            {
                erasPaleoController.GetComponent<ErasPaleoController>().SelectFirstItem();

                isOnMenu = false;
            }
            
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (instructionInterface.activeSelf)
            {
                instructionInterface.SetActive(false);
                audioSource.PlayOneShot(closeClip);

                erasPaleoController.SelectFirstItem();
            }
            else
            {
                TryReturnToCamp();
            }
        }

        // caso o usuario navege pelo tab e encontre uma célula ou item, o jogo despausa automaticamente.
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(EventSystem.current.currentSelectedGameObject.name.Contains("Animal") 
                || EventSystem.current.currentSelectedGameObject.name.Contains("Cell"))
            {
                paused = false;
            }
        }
    }

    public void ResetGameObjects()
    {
        SceneManager.LoadScene(ScenesNames.M009Eras);
    }

    public void CallHintMethod()
    {
        dicas.StartHints();
    }

    IEnumerator EndGameCoroutine()
    {
        LoseImage.SetActive(true);

        //ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_teia_derrota, LocalizationManager.instance.GetLozalization()));

        lifeExpController.AddEXP(0.0001f);

        audioSource.PlayOneShot(loseClip);

        yield return new WaitWhile(() => audioSource.isPlaying);

        //ReadText(LoseText);

        StartCoroutine(ReturnToCampCoroutine()); // volta para o navio
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. 
        //Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);
        //RestartTimer();
    }

    public void ReadMJMenu()
    {
        lifeExpController.ReadHPandEXP();
        //Debug.Log("Restam " + timer.text + " para finalizar o minijogo.");
        //ReadText("Restam " + timer.text + " para finalizar o minijogo.");
    }

    public void TryReturnToCamp()
    {
        paused = true;

        confirmQuit.SetActive(true);

        //ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        //ReadText(confirmQuit.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);
        confirmQuit.GetComponentInChildren<Button>().Select();

        audioSource.PlayOneShot(avisoClip);
    }

    public void ReturnToCamp()
    {
        //confirmQuit.SetActive(false);

        if (!PlayerPreferences.M009_Eras) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo

        SceneManager.LoadScene(ScenesNames.M009Camp);
    }

    public void ReturnToMJ()
    {
        confirmQuit.SetActive(false);

        erasPaleoController.SelectFirstItem();

    }

    public IEnumerator ReturnToCampCoroutine()
    {
        yield return new WaitForSeconds(4f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M009Camp);
    }
}
