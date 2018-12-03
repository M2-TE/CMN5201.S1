using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AssetManager
{
    private readonly string savefilePath = "/savefile.sfl";
    private readonly string assetBundlePath = "Assets/Asset Bundles/StandaloneWindows/";
    
    private readonly string settingsPath = "settings";
    private readonly string playableCharactersPath = "characters/main characters";
    
    #region Getters/Setters
    private AssetBundle settings;
    public AssetBundle Settings
    {
        get
        {
            if (settings != null) return settings;
            else return settings = AssetBundle.LoadFromFile(assetBundlePath + settingsPath);
        }
        set
        {
            if (value == null) settings.Unload(true);
        }
    }

    private AssetBundle characters;
    public AssetBundle Characters
    {
        get
        {
            if (characters != null) return characters;
            else return characters = AssetBundle.LoadFromFile(assetBundlePath + playableCharactersPath);
        }

        set
        {
            if (value == null) characters.Unload(true);
        }
    }
    #endregion

    public void Save(Savefile savefile)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + savefilePath;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, savefile);
        stream.Close();
    }

    public Savefile Load()
    {
        string path = Application.persistentDataPath + savefilePath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Savefile savefile = formatter.Deserialize(stream) as Savefile;
            stream.Close();
            return savefile;
        }
        else
        {
            Debug.LogError("Savefile not found.");
            return null;
        }
    }

    #region Singleton Implementation
    private static AssetManager instance;
    public static AssetManager Instance
    { get { return (instance != null) ? instance : instance = new AssetManager(); } }
    private AssetManager()
    {
        settings = Settings;
    }
    #endregion
}