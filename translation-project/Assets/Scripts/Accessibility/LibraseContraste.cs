using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class LibraseContraste : AbstractScreenReader {

    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public GameObject moldura;
    public HighContrastSettings hcsettings;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(moldura.activeSelf) moldura.SetActive(false);
        }
    }
    public void PlayVideo()
    {
        StartCoroutine(StartVideo(false, ""));
    }

    IEnumerator StartVideo(bool isUrl, string url)
    {
        moldura.SetActive(true);
        Application.runInBackground = true;

        if (!isUrl)
        {
            videoPlayer.source = VideoSource.VideoClip;
        }
        else
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = url;
        }

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing video...");
            yield return null;
        }

        Debug.Log("Prepared...");
        videoPlayer.GetComponent<RawImage>().texture = videoPlayer.texture;
        videoPlayer.Play();

        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");

        moldura.SetActive(false);
    }

    public void SetHighContrastParameter(bool isOn)
    {
        hcsettings.SetHighAccessibility(isOn);

        if (Parameters.HIGH_CONTRAST)
            ReadText("Alto contraste ativado");
        else
            ReadText("Alto contraste desativado");
    }

    public void PlayLibrasVideo(GameObject parentName)
    {
        string url = Parameters.DIALOGUE_PATH;

        // get the number of gameobject name to combine with path
        int choiceNumber = int.Parse(Regex.Match(parentName.name, @"(\d+)").Value);

        url += VIDEUIManager.dialogue_path[choiceNumber];

        Debug.Log("url >>> " + url);

        if (url != string.Empty)
            StartCoroutine(StartVideo(true, url));
    }

    public void PlayLibrasVideo(int index)
    {
        string url = Parameters.DIALOGUE_PATH;

        url += VIDEUIManager.dialogue_path[index];

        Debug.Log("url >>> " + url);

        if (url != string.Empty)
            StartCoroutine(StartVideo(true, url));
    }
}