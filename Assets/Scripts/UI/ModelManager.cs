using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using SimpleJSON;

public class ModelManager : MonoBehaviour
{
    [SerializeField] public GameObject ModelButton_Template;

    [SerializeField] public GameObject[] Models;

    Action<string> _createModelCallback;

    // Start is called before the first frame update
    void Start()
    {
        CreateModelList();

        /*_createModelCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateModelRoutine(jsonArrayString));
        };*/

        //CreateModels();
    }

    public void CreateModels()
    {
        string userId = Server.Instance.UserInfo.UserID;

        StartCoroutine(Server.Instance.Web.GetModelsID(userId, _createModelCallback));
    }

    /*IEnumerator CreateModelRoutine(string jsonArrayString)
    {
        //Parssin json array string as an array;
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            bool isDone = false;
            string id = jsonArray[i].AsObject["ID"];
            string modelId = jsonArray[i].AsObject["modelID"];
            JSONObject modelInfoJson = new JSONObject();

            //create a callback to get the infromation from Server.cs
            Action<string> getModelInfoCallback = (modelInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(modelInfo) as JSONArray;
                modelInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(Server.Instance.Web.GetModel(modelId, getModelInfoCallback));

            //Wait until the callback is called from WEB (info finished downloading)
            yield return new WaitUntil(() => isDone == true);

            //Instantiate Gameobject (model prefab)
            GameObject modelGo = Instantiate(ModelButton_Template);
            ModelInfo modelinfo = modelGo.AddComponent<ModelInfo>();

            modelinfo.ID = id;
            modelinfo.ModelID = modelId;

            modelGo.transform.SetParent(this.transform);
            modelGo.transform.localScale = Vector3.one;
            modelGo.transform.localPosition = Vector3.zero;

            //Fill Information
            modelGo.transform.Find("Name").GetComponent<TMP_Text>().text = modelInfoJson["name"];
            modelGo.transform.Find("Description").GetComponent<TMP_Text>().text = modelInfoJson["description"];


            //Downloading or Loading Model Icon.
            byte[] bytes = ImageManager.Instance.LoadImage(modelId);
            //Download from Web
            if(bytes.Length == 0)
            {
                Action<byte[]> getModelIconCallback = (downloadedBytes) =>
                {
                    Sprite sprite = ImageManager.Instance.BytesToSprite(downloadedBytes);
                    modelGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;
                    ImageManager.Instance.SaveImage(modelId, downloadedBytes);
                };
                StartCoroutine(Server.Instance.Web.GetModelIcon(modelId, getModelIconCallback));
            }
            else //Load from Device
            {
                Sprite sprite = ImageManager.Instance.BytesToSprite(bytes);
                modelGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;
            }

            // Set Delete Button
            modelGo.transform.Find("Delete").GetComponent<Button>().onClick.AddListener(() => {
                string iId = id;
                string mId = modelId;
                string uId = Server.Instance.UserInfo.UserID;
                StartCoroutine(Server.Instance.Web.DeleteModel(iId, mId, uId));
            });

            //continue to the next item
        }
    }*/

    public void CreateModelList()
    {
        for (int i = 0; i < Models.Length; i++)
        {
            GameObject model = Models[i].gameObject;

            //Create local variables
            string id = model.GetComponent<ModelInfo>().ID;
            string name = model.GetComponent<ModelInfo>().Name;
            string descritpion = model.GetComponent<ModelInfo>().Description;
            Sprite sprite = model.GetComponent<ModelInfo>().Sprite;

            //Instantiate Gameobject (model prefab)
            GameObject modelGo = Instantiate(ModelButton_Template);

            modelGo.transform.SetParent(this.transform);
            modelGo.transform.localScale = Vector3.one;
            modelGo.transform.localPosition = Vector3.zero;

            //Fill Information
            modelGo.transform.Find("Name").GetComponent<TMP_Text>().text = name;
            modelGo.transform.Find("Description").GetComponent<TMP_Text>().text = descritpion;
            modelGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;
            modelGo.GetComponent<ARmodel>().GameObject = model;

            //continue to the next item
        }
    }
}
