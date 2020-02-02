using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DBConnection : MonoBehaviour
{
    // server path
    // DO NOT UPLOAD THIS TO GITHUB
    private readonly string connection_url = "http://acessivel.ufabc.edu.br/antartica/php/index.php";
    private readonly string register_url   = "http://acessivel.ufabc.edu.br/antartica/php/registeruser.php";
    private readonly string password_url   = "http://acessivel.ufabc.edu.br/antartica/php/password.php";

    // local path
    //private readonly string connection_url = "http://localhost/antartica/index.php";
    //private readonly string register_url = "http://localhost/antartica/registeruser.php";
    //private readonly string password_url = "http://localhost/antartica/password.php";

    public static DBConnection instance;

    private void Start()
    {
        instance = this;

        //StartCoroutine(ConnectToDB());
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
                onComplete(ComparePasswords(passwordFromDB, passw));
            }
        }
    }

    private bool ComparePasswords(string passwordfromDB, string password)
    {
        if (SecurePasswordHasher.Verify(password, passwordfromDB))
        {
            Debug.Log("Log in successfull");
            return true;
        }

        Debug.Log("Wrong Credentials");
        return false;
    }
}
