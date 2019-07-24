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

    private void Start()
    {
        initialMinutes = 9f;
        initialSeconds = 59f;
    }

    private void Update()
    {
        HandleTimer();
    }

    public void ResetGameObjects()
    {
        SceneManager.LoadScene("TeiaAlimentarScene");
    }

    public void RestartTimer()
    {
        initialMinutes = 9f;
        initialSeconds = 59f;

        timer.text = "10:00";
    }

    public void HandleTimer()
    {
        elapsedMinutes = (int)initialMinutes - (int)(Time.timeSinceLevelLoad / 60f);
        elapsedSeconds = (int)initialSeconds - (int)(Time.timeSinceLevelLoad % 60f);

        timer.text = elapsedMinutes.ToString("00") + ":" + elapsedSeconds.ToString("00");

        // time is over
        if (elapsedMinutes < 0 || elapsedSeconds < 0)
        {
            timer.text = "00:00";
            // do something
            LoseImage.SetActive(true);
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
        SceneManager.LoadScene("ShipScene");
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
}
