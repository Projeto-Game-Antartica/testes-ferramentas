using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class YTVideoController : AbstractScreenReader, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Slider slider;

    private bool isSliding;
    
    private void Update()
    {
        if (!isSliding && videoPlayer.isPlaying)
            slider.value = (float) videoPlayer.frame/ videoPlayer.frameCount;
    }

    public void OnClick()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            ReadText("Vídeo pausado");
        }
        else
        {
            videoPlayer.Play();
            ReadText("Executando vídeo");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isSliding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float frame = (float)slider.value * videoPlayer.frameCount;
        videoPlayer.frame = (long)frame;
        isSliding = false;
    }
}
