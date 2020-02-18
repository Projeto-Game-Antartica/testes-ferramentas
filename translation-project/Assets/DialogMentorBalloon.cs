using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogMentorBalloon : MonoBehaviour
{
    public string MentorName;
    private string mentorId;

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        mentorId = sceneName + "_" + MentorName + "_DONE";

        if(PlayerPrefs.GetInt(mentorId, 0) == 1)
            SetDone();
    }

    public void SetDone() {
        PlayerPrefs.SetInt(mentorId, 1);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.4f, 1, 0.4f);
    }
}
