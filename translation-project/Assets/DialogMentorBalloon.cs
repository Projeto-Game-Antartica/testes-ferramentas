using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DialogMentorBalloon : MonoBehaviour
{
    public string MentorName;

    // Start is called before the first frame update
    void Start()
    {

        // switch(MentorName) {
        //     case "Mentor0":
        //         if(PlayerPreferences.M010_Mentor0_Talked)
        //             SetDone();
        //         break;

        //     case "Mentor3":
        //         if(PlayerPreferences.M010_Mentor3_Talked)
        //             SetDone();
        //         break;

        //     default:
        //         throw new NotImplementedException("Mentor name not implemented: " + MentorName);
        // }
        // Debug.Log("KEYSSSSSSS");
        // foreach(string key in PlayerPreferences.DoneDialogs.Keys) {
        //     Debug.Log("Key: " + key);
        // }
        if(MentorName != "" && PlayerPreferences.DoneDialogs.ContainsKey(MentorName) && PlayerPreferences.DoneDialogs[MentorName])
            SetDone(MentorName);
    }
    
    public void SetDone(string mentorName) {
        // switch(MentorName) {
        //     case "Mentor0":
        //         PlayerPreferences.M010_Mentor0_Talked = true;
        //         break;

        //     case "Mentor3":
        //         PlayerPreferences.M010_Mentor3_Talked = true;
        //         break;

        //     default:
        //         throw new NotImplementedException("Mentor name not implemented: " + MentorName);
        // }
        PlayerPreferences.DoneDialogs[MentorName] = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.4f, 1, 0.4f);
    }

    // public void SetDone() {
    //     switch(MentorName) {
    //         case "Mentor0":
    //             PlayerPreferences.M010_Mentor0_Talked = true;
    //             break;

    //         case "Mentor3":
    //             PlayerPreferences.M010_Mentor3_Talked = true;
    //             break;

    //         default:
    //             throw new NotImplementedException("Mentor name not implemented: " + MentorName);
    //     }
    //     gameObject.GetComponent<SpriteRenderer>().color = new Color(0.4f, 1, 0.4f);
    // }
}
