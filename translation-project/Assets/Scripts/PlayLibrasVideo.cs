using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class PlayLibrasVideo : MonoBehaviour {

    public VideoPlayer videoPlayer;
    public RawImage rawImage;

    public void PlayVideo()
    {
        Debug.Log("PlayVideo");
        Application.runInBackground = true;
        rawImage.gameObject.SetActive(true);
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
        rawImage.gameObject.SetActive(false);
    }
}
