using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeiaAlimentarScene : AbstractScreenReader {

    public TMPro.TextMeshProUGUI timer;
    
    // timer settings
    private float elapsedMinutes, elapsedSeconds, initialMinutes, initialSeconds;
    
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

    private float timerCount;
    private float timeInSeconds;
    private int timeInMinutes;

    public GameObject instructionInterface;
    public GameObject confirmQuit;

    //public MinijogosDicas dicas;

    public AudioSource audioSource;
    public AudioClip loseClip;
    public AudioClip closeClip;
    public AudioClip avisoClip;

    public TeiaAlimentarController teiaAlimentarController;

    public void InitializeGame()
    {
        StartCoroutine(StartTimer());
    }

    public IEnumerator StartTimer()
    {
        // read audiodescription

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_teia, LocalizationManager.instance.GetLozalization()));

        // start counting time
        yield return new WaitForSeconds(0.5f);

        initialMinutes = 29f;
        initialSeconds = 59f;

        timerCount = 0;

        started = true;

        resetButton.interactable = true;
        backButton.interactable = true;

        // start afther time seconds and repeat at repeatRate rate
        //InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);

        teiaAlimentarController.started = true;
        teiaAlimentarController.ReturnFirstCell().GetComponent<Selectable>().Select();
    }

    private void Update()
    {
        if(started && !finished && !paused) HandleTimer();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!instructionInterface.activeSelf)
            {
                instructionInterface.SetActive(true);
                paused = true;
            }
            //else
            //{
            //    // Read Instructions
            //}
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
                paused = true;
            }
            else
            {
                teiaAlimentarController.SelectFirstItem();
                paused = false;
                isOnMenu = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (instructionInterface.activeSelf)
            {
                instructionInterface.SetActive(false);
                audioSource.PlayOneShot(closeClip);

                teiaAlimentarController.SelectFirstItem();

                paused = false;
            }
            else
            {
                TryReturnToShip();
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

        if(Input.GetKeyDown(InputKeys.AUDIODESCRICAO_KEY))
        {
            ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_teia, LocalizationManager.instance.GetLozalization()));
        }
    }

    public void ResetGameObjects()
    {
        SceneManager.LoadScene(ScenesNames.M004TeiaAlimentar);
    }

    public void RestartTimer()
    {
        timer.text = "30:00";

        initialMinutes = 29f;
        initialSeconds = 59f;

    }

    //public void CallHintMethod()
    //{
    //    dicas.StartHints();
    //}

    public void HandleTimer()
    {
        timerCount += Time.deltaTime;

        elapsedMinutes = (int)initialMinutes - (int)(timerCount / 60f);
        elapsedSeconds = (int)initialSeconds - (int)(timerCount % 60f);

        //elapsedMinutes = (int)initialMinutes - (int)(Time.timeSinceLevelLoad / 60f);
        //elapsedSeconds = (int)initialSeconds - (int)(Time.timeSinceLevelLoad % 60f);

        timer.text = elapsedMinutes.ToString("00") + ":" + elapsedSeconds.ToString("00");

        // time is over
        if (elapsedMinutes < 0 || elapsedSeconds < 0)
        {
            finished = true;
            StartCoroutine(EndGameCoroutine());
        }
    }

    IEnumerator EndGameCoroutine()
    {
        Debug.Log("Coroutine.");
        timer.text = "00:00";
        // do something
        LoseImage.SetActive(true);

        ReadText(LoseText);

        lifeExpController.AddEXP(PlayerPreferences.XPlosePuzzle);

        audioSource.PlayOneShot(loseClip);

        yield return new WaitWhile(() => audioSource.isPlaying);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_m004_teia_derrota, LocalizationManager.instance.GetLozalization()));

        StartCoroutine(ReturnToShipCoroutine()); // volta para o navio
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
    
        RestartTimer();
    }

    public void ReadMJMenu()
    {
        lifeExpController.ReadHPandEXP();

        if (elapsedMinutes > 0)
        {
            ReadText("Restam " + elapsedMinutes + " minutos e " + elapsedSeconds + " segundos para terminar o minijogo.");
            Debug.Log("Restam " + elapsedMinutes + " minutos e " + elapsedSeconds + " segundos para terminar o minijogo.");
        }
        else
        {
            ReadText("Restam " + elapsedSeconds + " segundos para terminar o minijogo.");
            Debug.Log("Restam " + elapsedSeconds + " segundos para terminar o minijogo.");
        }
    }

    public void TryReturnToShip()
    {
        paused = true;

        confirmQuit.SetActive(true);

        ReadText(ReadableTexts.instance.GetReadableText(ReadableTexts.key_gameplay_aviso_botoes, LocalizationManager.instance.GetLozalization()));

        ReadText(confirmQuit.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);

        confirmQuit.GetComponentInChildren<Button>().Select();

        audioSource.PlayOneShot(avisoClip);
    }

    public void ReturnToShip()
    {
        confirmQuit.SetActive(false);

        if (!PlayerPreferences.M004_TeiaAlimentar) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo

        SceneManager.LoadScene(ScenesNames.M004Ship);
    }

    public void ReturnToMJ()
    {
        confirmQuit.SetActive(false);

        teiaAlimentarController.SelectFirstItem();

        paused = false;
    }

    public IEnumerator ReturnToShipCoroutine()
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(ScenesNames.M004Ship);
    }
}
