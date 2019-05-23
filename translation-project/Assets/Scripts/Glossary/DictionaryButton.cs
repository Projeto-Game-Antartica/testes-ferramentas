using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryButton : AbstractScreenReader
{
    public Button buttonComponent;
    public Text keyLabel;
    public DictionaryController dictionaryController;
    public AudioSource audioSource;
    public static bool contentButton;

    // key for playing the audio clip
    public static string keyAudio;

    // Use this for initialization
    void Start()
    {
        dictionaryController = buttonComponent.GetComponentInParent<DictionaryController>();
        buttonComponent.transform.localScale = Vector3.one;
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void HandleClick()
    {
        dictionaryController.ShowDescriptionContent(keyLabel.text);
    }

    public void PlayAudio()
    {
        if (audioSource.isPlaying) audioSource.Stop();

        audioSource.PlayOneShot(dictionaryController.GetAudioClip(keyAudio, LocalizationManager.instance.GetLozalization()));
    }

    public bool IsAudioPlaying()
    {
        bool isPlaying = false;

        if (audioSource.isPlaying)
            isPlaying = true;

        return isPlaying;
    }

    public void SetContentButton(bool content)
    {
        contentButton = content;
    }

    public void ReadButton()
    {
        ReadText(keyLabel.text);
    }
}
