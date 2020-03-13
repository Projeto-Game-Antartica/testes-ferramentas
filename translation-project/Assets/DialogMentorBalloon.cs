using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DialogMentorBalloon : MonoBehaviour
{
    public string MentorName;
    public SpriteRenderer DialogBalloon;

    private string mentorId; //Unique ID per mission per mentor

    // Start is called before the first frame update
    void Start()
    {
        mentorId = MentorName + "_" + SceneManager.GetActiveScene().name;
        
        if(PlayerPreferences.DoneDialogs.ContainsKey(mentorId) && PlayerPreferences.DoneDialogs[mentorId])
            SetDialogDone();
    }

    public void SetDialogDone() {
        PlayerPreferences.DoneDialogs[mentorId] = true;
        DialogBalloon.color = new Color(0.4f, 1, 0.4f);
    }
}
