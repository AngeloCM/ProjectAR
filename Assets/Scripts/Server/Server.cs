using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Server : MonoBehaviour
{
    public static Server Instance;

    public Web Web;
    public UserInfo UserInfo;
    public Login Login;
    public ModelManager ModelManager;

    public GameObject UserProfile;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        //Get Web Component.
        Web = GetComponent<Web>();
        UserInfo = GetComponent<UserInfo>();

        ModelManager.gameObject.SetActive(false);

        Login.ARPanel.gameObject.SetActive(false);
        Login.ARCamera.gameObject.SetActive(false);
    }
}
