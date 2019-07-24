using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class MentorController : MonoBehaviour {
    
    public AudioSource audioSource; // beep for localization
    private string mentorName;

    public SpriteRenderer[] spriteRenderer;

    private void Start()
    {
        mentorName = gameObject.name;
        //Debug.Log(mentorName);

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
        //int index = Random.Range(0, RandomMentorDialogues.GetVectorLenght(mentorName));
        ////Debug.Log(index);

        //// change the assigned dialogue
        //GetDialogue(mentorName, index);
        float xdif = gameObject.transform.position.x - collision.gameObject.transform.position.x;
        //Debug.Log(gameObject.transform.position - collision.gameObject.transform.position);

        if(xdif > 0)
        {
            foreach(SpriteRenderer sp in spriteRenderer)
                sp.flipX = true;
        }
        else
        {
            foreach (SpriteRenderer sp in spriteRenderer)
                sp.flipX = false;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    int index = Random.Range(0, RandomMentorDialogues.GetVectorLenght(mentorName));
    //    //Debug.Log(index);

    //    // change the assigned dialogue
    //    GetDialogue(mentorName, index);
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    int index = Random.Range(0, RandomMentorDialogues.GetVectorLenght(mentorName));
    //    //Debug.Log(index);

    //    // change the assigned dialogue
    //    GetDialogue(mentorName, index);
    //}

    private void GetDialogue(string mentorName, int index)
    {
        GetComponent<VIDE_Assign>().assignedDialogue = RandomMentorDialogues.GetRandomDialogue(mentorName, index);
    }

}
