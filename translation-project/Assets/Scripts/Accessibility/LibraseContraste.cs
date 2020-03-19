using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class LibraseContraste : AbstractScreenReader {

    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public GameObject moldura;
    public HighContrastSettings hcsettings;

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

    public void PlayDialogueVideo()
    {
        string url = Parameters.DIALOGUE_PATH + VIDEUIManager.dialogue_video_url + Parameters.MP4_TYPE;

        Debug.Log("url >>> " + url);
        if (url != string.Empty)
            StartCoroutine(StartVideo(true, url));
    }
}