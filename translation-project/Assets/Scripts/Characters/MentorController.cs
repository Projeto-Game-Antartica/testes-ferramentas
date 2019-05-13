using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class MentorController : MonoBehaviour {
    
    public AudioSource audioSource; // beep for localization
    private string mentorName;

    private void Start()
    {
        mentorName = gameObject.name;
        Debug.Log(mentorName);

        if (Parameters.ACCESSIBILITY)
        {
            audioSource.playOnAwake = true;
            audioSource.Play();
            audioSource.loop = true;
        }
    }

    private void Update()
    {
        if (Parameters.ACCESSIBILITY)
            PlayAcessibilityAudio();
        else if (audioSource.isPlaying)
            audioSource.Stop();
    }

    private void PlayAcessibilityAudio()
    {
        audioSource.playOnAwake = true;
        if (!audioSource.isPlaying) audioSource.Play();
        audioSource.loop = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int index = Random.Range(0, RandomMentorDialogues.GetVectorLenght(mentorName));
        Debug.Log(index);

        // change the assigned dialogue
        //GetDialogue(mentorName, index);
    }

    private void GetDialogue(string mentorName, int index)
    {
        GetComponent<VIDE_Assign>().assignedDialogue = RandomMentorDialogues.GetRandomDialogue(mentorName, index);
    }

}
