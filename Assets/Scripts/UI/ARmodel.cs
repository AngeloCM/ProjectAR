using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARmodel : MonoBehaviour
{
    public GameObject GameObject;

    public void SetGameobject()
    {
        ARController.Instance.Webmodel = GameObject;

        Server.Instance.ModelManager.gameObject.SetActive(false);
        Server.Instance.Login.CanvasCamera.gameObject.SetActive(false);

        Server.Instance.Login.ARPanel.gameObject.SetActive(true);
        Server.Instance.Login.ARCamera.gameObject.SetActive(true);
    }
}
