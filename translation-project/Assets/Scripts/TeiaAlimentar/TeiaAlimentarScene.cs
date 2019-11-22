using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeiaAlimentarScene : AbstractScreenReader {

    public Text timer;
    
    // timer settings
    private float elapsedMinutes, elapsedSeconds, initialMinutes, initialSeconds;
    
    public GameObject LoseImage;
    public TMPro.TextMeshProUGUI LoseText;

    public LifeExpController lifeExpController;

    public Button audioButton;
    public Button resetButton;
    public Button backButton;

    private bool started = false;
    private bool finished = false;

    private float timerCount;
    private float timeInSeconds;
    private int timeInMinutes;

    public GameObject instructionInterface;

    public MinijogosDicas dicas;

    public AudioSource audioSource;
    public AudioClip loseClip;

    public void StartTimer()
    {
        initialMinutes = 2f;
        initialSeconds = 59f;

        timerCount = 0;

        started = true;

        resetButton.interactable = true;
        backButton.interactable = true;

        // start afther time seconds and repeat at repeatRate rate
        InvokeRepeating("CallHintMethod", dicas.time, dicas.repeatRate);
    }

    private void Update()
    {
        if(started && !finished) HandleTimer();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!instructionInterface.activeSelf)
                instructionInterface.SetActive(true);
            else
                instructionInterface.SetActive(false);
        }

        if(Input.GetKeyDown(InputKeys.MJMENU_KEY))
        {
            ReadMJMenu();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            audioButton.Select();
        }
    }

    public void ResetGameObjects()
    {
        SceneManager.LoadScene(ScenesNames.M004TeiaAlimentar);
    }

    public void RestartTimer()
    {
        initialMinutes = 2f;
        initialSeconds = 59f;

        timer.text = "2:00";
    }

    public void CallHintMethod()
    {
        dicas.StartHints();
    }

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

        lifeExpController.AddEXP(0.0001f);

        audioSource.PlayOneShot(loseClip);

        yield return new WaitWhile(() => audioSource.isPlaying);

        ReadText(LoseText);

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
        Debug.Log(mode);
        RestartTimer();
    }

    public void ReadMJMenu()
    {
        lifeExpController.ReadHPandEXP();
        Debug.Log("Restam " + timer.text + " para finalizar o minijogo.");
        ReadText("Restam " + timer.text + " para finalizar o minijogo.");
    }

    public void ReturnToShip()
    {
        if (!PlayerPreferences.M004_TeiaAlimentar) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
        SceneManager.LoadScene(ScenesNames.M004Ship);
    }

    public IEnumerator ReturnToShipCoroutine()
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(ScenesNames.M004Ship);
    }
}
