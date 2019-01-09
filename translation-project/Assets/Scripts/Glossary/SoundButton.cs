using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    public Button buttonComponent;
    public Text keyLabel;
    public AudioSource audioSource;
    public SoundGlossaryController soundGlossaryController;

    // Use this for initialization
    void Start()
    {
        soundGlossaryController = buttonComponent.GetComponentInParent<SoundGlossaryController>();
        audioSource = buttonComponent.GetComponentInParent<AudioSource>();
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void HandleClick()
    {
        if (audioSource.isPlaying) audioSource.Stop();

        audioSource.PlayOneShot(soundGlossaryController.GetAudioClip(keyLabel.text, LocalizationManager.instance.GetLozalization()));
    }

    public void ReadButton()
    {
        TolkUtil.Speak(keyLabel.text);
    }
}
