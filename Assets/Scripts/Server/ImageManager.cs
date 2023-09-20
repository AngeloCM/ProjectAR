using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageManager : MonoBehaviour
{
    public static ImageManager Instance;
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
        if(Instance != null)
        {
            GameObject.Destroy(this);
            return;
        }
        Instance = this;

        _basePath = Application.persistentDataPath + "/Images/";
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    bool ImageExists(string name)
    {
        return File.Exists(_basePath + name);
    }

    public void SaveImage(string name, byte[] bytes)
    {
        File.WriteAllBytes(_basePath + name, bytes);
    }

    public byte[] LoadImage(string name)
    {
        byte[] bytes = new byte[0];

        if (ImageExists(name))
        {
            bytes = File.ReadAllBytes(_basePath + name);
        }

        return bytes;
    }

    public Sprite BytesToSprite(byte[] bytes)
    {
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        //Create sprite (to be placed in UI)
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }
}
