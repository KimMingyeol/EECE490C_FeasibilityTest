using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;

public class LogInSerializer {
    public string username;
    public string password;
    public string token;
}

public class LogInHandler : MonoBehaviour
{
    public Button logInButton;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    private string hostURL = "http://127.0.0.1:8000";

    void Start()
    {
        logInButton.onClick.AddListener(initiateLogIn);   
    }

    void Update()
    {
    }

    private void initiateLogIn() {
        string api = "/photo_server/login/";
        string username = usernameInput.text;
        string password = passwordInput.text;
        StartCoroutine(logIn(hostURL + api, username, password));
    }

    private IEnumerator logIn(string logInURL, string username, string password) {
        WWWForm logInForm = new WWWForm();
        logInForm.AddField("username", username);
        logInForm.AddField("password", password);
        logInForm.AddField("token", "");
        UnityWebRequest logInRequest = UnityWebRequest.Post(logInURL, logInForm);
        Debug.Log("LOG IN: Sending Request to " + logInURL);
        yield return logInRequest.SendWebRequest();

        if (logInRequest.result == UnityWebRequest.Result.Success) {
            LogInSerializer logInSerializer = JsonConvert.DeserializeObject<LogInSerializer>(logInRequest.downloadHandler.text);
            UserState.logIn = true;
            UserState.username = logInSerializer.username;
            UserState.token = logInSerializer.token;
            Debug.Log("LOG IN: Receive and Save Token " + UserState.token);
        } else {
            Debug.Log("Error while logging in");
            if (logInRequest.responseCode == 401) {
                Debug.Log("Login Failed: Wrong username or password");
            }
        }

        logInRequest.Dispose(); // else, memory leak?
    }
}
