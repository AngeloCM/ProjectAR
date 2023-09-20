using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BundleWebLoader : MonoBehaviour
{
    public string assetID;

    private void Start()
    {
        //assetID = GetComponent<ModelInfo>().ModelID;
    }

    public void InstatiateWebAsset()
    {
        //TODO
        //Save AssetBundle and Load from storage;

        Action<AssetBundle> loadPrefab = (prefab) =>
        {
            ARController.Instance.Webmodel = prefab.LoadAsset(assetID) as GameObject;
            prefab.Unload(false);
        };

        //StartCoroutine(Server.Instance.Web.GetPrefabModel(assetID, loadPrefab));
    }
}
