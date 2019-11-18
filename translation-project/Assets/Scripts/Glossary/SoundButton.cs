using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : AbstractScreenReader
{
    public Button buttonComponent;
    public TMPro.TextMeshProUGUI keyLabel;
    public SoundGlossaryController soundGlossaryController;
    public static AudioSource audioSource;
    public static bool contentButton;

    public bool first = false;
    public bool last = false;

    // Use this for initialization
    void Start()
    {
        soundGlossaryController = buttonComponent.GetComponentInParent<SoundGlossaryController>();
        buttonComponent.transform.localScale = Vector3.one;
        audioSource = buttonComponent.GetComponentInParent<AudioSource>();
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void HandleClick()
    {
        if (audioSource.isPlaying) audioSource.Stop();

        audioSource.PlayOneShot(soundGlossaryController.GetAudioClip(keyLabel.text, LocalizationManager.instance.GetLozalization()));
    }

    public void SetContentButton(bool content)
    {
        contentButton = content;
    }

    public static bool IsAudioPlaying()
    {
        bool isPlaying = false;

        if (audioSource.isPlaying)
            isPlaying = true;

        return isPlaying;
    }

    public void ReadButton()
    {
        ReadText(keyLabel.text);

        if (first && last)
            ReadText("Único item da lista");
        else if (first)
            ReadText("Primeiro item da lista");
        else if (last)
            ReadText("Último item da lista");
    }
}
