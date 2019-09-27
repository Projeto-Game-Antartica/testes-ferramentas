using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeiaAlimentarScene : MonoBehaviour {

    public Text timer;

    public Slider audioSlider;

    // timer settings
    private float elapsedMinutes, elapsedSeconds, initialMinutes, initialSeconds;
    
    public GameObject LoseImage;

    public LifeExpController lifeExpController;

    public Button resetButton;
    public Button backButton;

    private bool started = false;

    private float timerCount;
    private float timeInSeconds;
    private int timeInMinutes;

    public GameObject instructionInterface;

    public void StartTimer()
    {
        initialMinutes = 9f;
        initialSeconds = 59f;

        timerCount = 0;

        started = true;

        resetButton.interactable = true;
        backButton.interactable = true;
    }

    private void Update()
    {
        if(started) HandleTimer();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!instructionInterface.activeSelf)
                instructionInterface.SetActive(true);
            else
                instructionInterface.SetActive(false);
        }
    }

    public void ResetGameObjects()
    {
        SceneManager.LoadScene(ScenesNames.M004TeiaAlimentar);
    }

    public void RestartTimer()
    {
        initialMinutes = 9f;
        initialSeconds = 59f;

        timer.text = "10:00";
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
            timer.text = "00:00";
            // do something
            LoseImage.SetActive(true);
            lifeExpController.AddEXP(0.0001f);

            StartCoroutine(ReturnToShipCoroutine()); // volta para o navio
        }
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

    public void ReturnToShip()
    {
        if (!PlayerPreferences.M004_TeiaAlimentar) lifeExpController.RemoveEXP(0.0001f); // saiu sem concluir o minijogo
        SceneManager.LoadScene(ScenesNames.M004Ship);
    }

    public void ActivateAudioSlider()
    {
        if(audioSlider.IsActive())  
        {
            audioSlider.gameObject.SetActive(false);
        }
        else
        {
            audioSlider.gameObject.SetActive(true);
        }
    }

    public IEnumerator ReturnToShipCoroutine()
    {
        yield return new WaitForSeconds(7f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesNames.M004Ship);
    }
}
