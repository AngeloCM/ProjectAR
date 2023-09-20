using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;

    [SerializeField] TMP_Text errorMessage;

    [SerializeField] GameObject progressCircle;
    [SerializeField] Button loginButton;

    [SerializeField] GameObject ModelPanel;
    [SerializeField] GameObject LoginPanel;
    [SerializeField] public GameObject ARPanel;


    [SerializeField] public Camera CanvasCamera;
    [SerializeField] public Camera ARCamera;

    void Start()
    {
        loginButton.onClick.AddListener(() => {
            OnLoginButtonClicked();

            //In case of returning to web requests
            //StartCoroutine(Server.Instance.Web.Login(username.text, password.text));

            //Start the App once clicked;
            Server.Instance.UserProfile.SetActive(true);
            Server.Instance.Login.gameObject.SetActive(false);
        });
    }

    public void OnLoginButtonClicked()
    {
        loginButton.interactable = false;
        progressCircle.SetActive(true);
    }

    public void OnLoginButtonUnClicked()
    {
        loginButton.interactable = true;
        progressCircle.SetActive(false);
    }

    public void OnLoginMessage(string message)
    {
        if (message != null)
        {
            errorMessage.text = message;
        }
        else
        {
            errorMessage.text = " ";
        }
            
    }
}
