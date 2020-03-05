using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DBConnection : MonoBehaviour
{
    // local path
    private readonly string connection_url = "http://acessivel.ufabc.edu.br/antartica/php/index.php";
    private readonly string register_url = "http://acessivel.ufabc.edu.br/antartica/php/registeruser.php";
    private readonly string password_url = "http://acessivel.ufabc.edu.br/antartica/php/password.php";
    private readonly string username_url = "http://acessivel.ufabc.edu.br/antartica/php/username.php";

    public static DBConnection instance;

    private void Start()
    {
        instance = this;

        //StartCoroutine(ConnectToDB());

        DontDestroyOnLoad(this);
    }

    private IEnumerator ConnectToDB()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(connection_url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
                Debug.Log(www.error);
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator RegisterUser(string name, string email, string passw, DateTime dateTime, Action<bool> onComplete)
    {
        Debug.Log("registering user...");

        WWWForm form = new WWWForm();

        form.AddField("loginName", name);
        form.AddField("loginEmail", email);
        form.AddField("loginPassw", passw);
        form.AddField("dateTime", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));

        using (UnityWebRequest www = UnityWebRequest.Post(register_url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                onComplete(false);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                PlayerPreferences.PlayerName = name;

                onComplete(true);
            }
        }
    }

    public IEnumerator TryLogIn(string email, string passw, DateTime dateTime, Action<bool> onComplete)
    {
        Debug.Log("trying to log in...");

        WWWForm form = new WWWForm();
        
        form.AddField("loginEmail", email);
        form.AddField("loginPassw", passw);
        form.AddField("dateTime", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));

        using (UnityWebRequest www = UnityWebRequest.Post(password_url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
                Debug.Log(www.error);
            else
            {
                //Debug.Log(www.downloadHandler.text);

                string passwordFromDB = www.downloadHandler.text;
                onComplete(ComparePasswords(email, passwordFromDB, passw));
            }
        }
    }

    private bool ComparePasswords(string email, string passwordfromDB, string password)
    {
        DateTime dateTime = DateTime.Now;
        if (SecurePasswordHasher.Verify(password, passwordfromDB))
        {
            Debug.Log("Log in successfull");
            StartCoroutine(GetUserName(email, dateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            return true;
        }

        Debug.Log("Wrong Credentials");
        return false;
    }

    private IEnumerator GetUserName(string email, string date)
    {
        WWWForm form = new WWWForm();

        form.AddField("loginEmail", email);
        form.AddField("dateTime", date);

        using (UnityWebRequest www = UnityWebRequest.Post(username_url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                PlayerPreferences.PlayerName = www.downloadHandler.text;
            }
        }
    }
}
