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
    private readonly string equipmentPath = "equipment";
    private readonly string skillsPath = "skills";

    #region Getters/Setters
    private Camera mainCam;
    public Camera MainCam
    {
        get
        {
            if (mainCam != null) return mainCam;
            else return mainCam = Camera.main;
        }
    }
    
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

    private AssetBundle equipment;
    public AssetBundle Equipment
    {
        get
        {
            if (equipment != null) return equipment;
            else return equipment = AssetBundle.LoadFromFile(assetBundlePath + equipmentPath);
        }

        set
        {
            if (value == null) equipment.Unload(true);
        }
    }

    private AssetBundle skills;
    public AssetBundle Skills
    {
        get
        {
            if (skills != null) return skills;
            else return skills = AssetBundle.LoadFromFile(assetBundlePath + skillsPath);
        }

        set
        {
            if (value == null) skills.Unload(true);
        }
    }
    #endregion

    public void Save(Savestate savefile)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + savefilePath;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, savefile);
        stream.Close();
    }

    public Savestate Load()
    {
        string path = Application.persistentDataPath + savefilePath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Savestate savefile = formatter.Deserialize(stream) as Savestate;
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