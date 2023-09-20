using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
    [SerializeField] string getData;
    [SerializeField] string getUsers;
    [SerializeField] string login;
    [SerializeField] string register;
    [SerializeField] string getModelsID;
    [SerializeField] string getModel;
    [SerializeField] string deleteModel;
    [SerializeField] string getModelIcon;
    [SerializeField] string getPrefabModel;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void ShowUserItems()
    {
        //StartCoroutine(GetModelsID(Server.Instance.UserInfo.UserID));
    }

    public IEnumerator GetData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(getData))
        {
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }

    public IEnumerator GetUsers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(getUsers))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }


    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post(login, form))
        {
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(1f);

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                Server.Instance.UserInfo.SetCredentials(username, password);
                Server.Instance.UserInfo.SetID(www.downloadHandler.text);

                //if we logged corretly.
                if (www.downloadHandler.text.Contains("Wrong Credentials") || www.downloadHandler.text.Contains("Username does not exists"))
                {
                    Debug.Log("Try Again !");
                    Server.Instance.Login.OnLoginMessage(www.downloadHandler.text);
                    Server.Instance.Login.OnLoginButtonUnClicked();
                }
                else
                {
                    Server.Instance.UserProfile.SetActive(true);
                    Server.Instance.Login.gameObject.SetActive(false);
                }
            }
        }
    }

    public IEnumerator RegisterUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post(register, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetModelsID(string userID, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post(getModelsID, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }
    }

    public IEnumerator GetModel(string modelID, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("modelID", modelID);

        using (UnityWebRequest www = UnityWebRequest.Post(getModel, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }
    }

    public IEnumerator DeleteModel(string id, string modelID, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("modelID", modelID);
        form.AddField("userID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post(deleteModel, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetModelIcon(string modelID, System.Action<byte[]> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("modelID", modelID);

        using (UnityWebRequest www = UnityWebRequest.Post(getModelIcon, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("DOWNLOADING ICON: " + modelID);
                byte[] bytes = www.downloadHandler.data;
                callback(bytes);
            }
        }
    }

    public IEnumerator GetPrefabModel(string assetID, System.Action<AssetBundle> callback)
    {
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(getPrefabModel + assetID))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("error");
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("DOWNLOADING PREFAB !: " + assetID);
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

                if(bundle != null)
                {
                    //Turn off ModelManager Canvas;
                    Server.Instance.ModelManager.gameObject.SetActive(false);
                    Server.Instance.Login.CanvasCamera.gameObject.SetActive(false);

                    Server.Instance.Login.ARPanel.gameObject.SetActive(true);
                    Server.Instance.Login.ARCamera.gameObject.SetActive(true);

                    callback(bundle);
                }
                else
                {
                    Debug.Log("Deu RUIM");
                    yield break;
                }
            }
        }   
    }

}
