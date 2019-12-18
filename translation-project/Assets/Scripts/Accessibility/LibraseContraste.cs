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
        Debug.Log("PlayVideo");
        Application.runInBackground = true;
        moldura.SetActive(true);
        StartCoroutine(StartVideo());
    }

    IEnumerator StartVideo()
    {
        ReadText("Vídeo de libras aberto");
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

        ReadText("Vídeo de libras fechado");
    }

    public void SetHighContrastParameter(bool isOn)
    {
        hcsettings.SetHighAccessibility(isOn);

        if (Parameters.HIGH_CONTRAST)
            ReadText("Alto contraste ativado");
        else
            ReadText("Alto contraste desativado");
    }
}
