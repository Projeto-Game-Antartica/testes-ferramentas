using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class LibraseContraste : MonoBehaviour {

    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public GameObject moldura;

    public void PlayVideo()
    {
        Debug.Log("PlayVideo");
        Application.runInBackground = true;
        moldura.SetActive(true);
        StartCoroutine(StartVideo());
    }

    IEnumerator StartVideo()
    {
        videoPlayer.Prepare();
        
        while(!videoPlayer.isPrepared)
        {
            yield return null;
        }

        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();

        Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");
        moldura.SetActive(false);
    }

    public void SetHighContrastParameter()
    {
        if (Parameters.HIGH_CONTRAST)
            Parameters.HIGH_CONTRAST = false;
        else
            Parameters.HIGH_CONTRAST = true;

        Debug.Log("HC " + Parameters.HIGH_CONTRAST);
    }
}
