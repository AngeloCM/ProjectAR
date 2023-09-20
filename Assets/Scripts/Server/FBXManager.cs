using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FBXManager : MonoBehaviour
{
    public static FBXManager Instance;
    string _basePath;


    //TODO:
    // 0. make a base path;
    // 1. Check if Image already exists;
    // 2. Save Images;
    // 3. Load Images (IO);
    // 4. Try to get Image;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this);
            return;
        }
        Instance = this;

        _basePath = Application.persistentDataPath + "/FBX/";
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    bool FBXExists(string assetID)
    {
        return File.Exists(_basePath + assetID);
    }

    public void SaveFBX(string name, AssetBundle ab)
    {
        //File.WriteAllBytes(_basePath + name, bytes);
    }

    /*public AssetBundle LoadFBX(string assetID)
    {
        AssetBundle assetBundle = null;

        if (FBXExists(assetID))
        {
            assetBundle = (AssetBundle)AssetDatabase.LoadAssetAtPath(_basePath, typeof(AssetBundle));
        }

        return assetBundle;
    }*/
}
