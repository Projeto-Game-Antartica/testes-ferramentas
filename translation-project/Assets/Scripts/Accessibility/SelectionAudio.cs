using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionAudio : AbstractScreenReader {
    
    public AudioClip selectClip;

    public void PlaySelectionAudio()
    {
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();

            UnityEngine.Audio.AudioMixer audioMixer = Resources.Load("Audio/AudioMixer") as UnityEngine.Audio.AudioMixer;
            GetComponent<AudioSource>().outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];
        }

        PlaySelectAudio(GetComponent<AudioSource>(), selectClip);
    }
}
