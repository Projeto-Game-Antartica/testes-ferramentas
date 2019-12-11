using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VIDE_Data;

public class MentorController : MonoBehaviour {
    
    public AudioSource audioSource; // beep for localization
    private string mentorName;

    public string missionNumber;
    
    public SpriteRenderer minijogoBalloon;

    public SpriteRenderer[] spriteRenderer;

    private int[] mentorIndexes;
    private int count;

    private void Start()
    {
        count = 0;
        mentorName = gameObject.name;
        mentorIndexes = new int[MentorDialogues.GetVectorLenght(missionNumber, mentorName)];

        Debug.Log(mentorName + " " + MentorDialogues.GetVectorLenght(missionNumber, mentorName));
        for (int i = 0; i < MentorDialogues.GetVectorLenght(missionNumber, mentorName); i++)
            mentorIndexes[i] = i;

        //Debug.Log(mentorName);

        if (Parameters.ACCESSIBILITY && !VD.isActive)
        {
            PlayAcessibilityAudio();
        }

        HandleMinijogoBalloonColor(mentorName);
    }

    private void Update()
    {
        if (Parameters.ACCESSIBILITY && !VD.isActive)
            PlayAcessibilityAudio();
        else
            audioSource.Stop();
    }

    private void HandleMinijogoBalloonColor(string mentorName)
    {
        // mentores com minijogos
        switch(mentorName)
        {
            case "Mentor0":
                if (PlayerPreferences.M004_Memoria) minijogoBalloon.color = new Color(0.4f, 1, 0.4f);
                else minijogoBalloon.color = new Color(0.3f, 0.7f, 1);
                break;
            case "Mentor2":
                if (PlayerPreferences.M004_TeiaAlimentar) minijogoBalloon.color = new Color(0.4f, 1, 0.4f);
                else minijogoBalloon.color = new Color(0.3f, 0.7f, 1);
                break;
            case "Mentor4":
                if (PlayerPreferences.M004_FotoIdentificacao) minijogoBalloon.color = new Color(0.4f, 1, 0.4f);
                else minijogoBalloon.color = new Color(0.3f, 0.7f, 1);
                break;
        }
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
        Debug.Log(mentorIndexes[count]);

        // change the assigned dialogue
        GetDialogue(missionNumber, mentorName, mentorIndexes[count]);

        // sum the counter
        count++;

        // if is the end of vector, start over
        if (count >= mentorIndexes.Length)
            count = 0;

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

    private void GetDialogue(string missionNumber, string mentorName, int index)
    {
        GetComponent<VIDE_Assign>().assignedDialogue = MentorDialogues.GetDialogue(missionNumber, mentorName, index);
    }

}
