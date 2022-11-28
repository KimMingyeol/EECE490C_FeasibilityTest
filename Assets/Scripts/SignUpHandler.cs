using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class SignUpHandler : MonoBehaviour
{
    public Button signUpButton;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField nicknameInput;
    private string hostURL = "http://127.0.0.1:8000";

    void Start()
    {
        signUpButton.onClick.AddListener(initiateSignUp);
    }

    void Update()
    {
    }

    private void initiateSignUp() {
        string api = "/photo_server/signup/";
        string username = usernameInput.text;
        string password = passwordInput.text;
        string nickname = nicknameInput.text;
        StartCoroutine(signUp(hostURL + api, username, password, nickname));
    }

    private IEnumerator signUp(string signUpURL, string username, string password, string nickname) {
        WWWForm signUpForm = new WWWForm();
        signUpForm.AddField("username", username);
        signUpForm.AddField("password", password);
        signUpForm.AddField("nickname", nickname);
        UnityWebRequest signUpRequest = UnityWebRequest.Post(signUpURL, signUpForm);
        Debug.Log("SIGN UP: Sending Request to " + signUpURL);
        yield return signUpRequest.SendWebRequest();

        if (signUpRequest.result == UnityWebRequest.Result.Success) {
            Debug.Log(signUpRequest.downloadHandler.text);
        } else {
            Debug.Log("Error while signing up");
            if (signUpRequest.responseCode == 409) {
                Debug.Log("User Already Exists");
            }
        }
        
        signUpRequest.Dispose(); // else, memory leak?
    }
}
