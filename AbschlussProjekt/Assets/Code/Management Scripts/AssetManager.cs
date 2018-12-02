using System;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager
{
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

    private AssetBundle playableCharacters;
    public AssetBundle PlayableCharacters
    {
        get
        {
            if (playableCharacters != null) return playableCharacters;
            else return playableCharacters = AssetBundle.LoadFromFile(assetBundlePath + playableCharactersPath);
        }

        set
        {
            if (value == null) playableCharacters.Unload(true);
        }
    }
    #endregion

    #region Probably Unnecessary AssetBundle Methods
    private T LoadSettings<T>(string name) where T : Settings
    {
        return Settings.LoadAsset<T>(name);
    }

    private T LoadCharacter<T>(string name) where T : PlayableCharacter
    {
        return PlayableCharacters.LoadAsset<T>(name);
    }

    private void LogCurrentlyLoadedBundles()
    {
        foreach(AssetBundle bundle in AssetBundle.GetAllLoadedAssetBundles())
            Debug.Log(bundle);
    }

    private void Unload()
    {
        Resources.UnloadUnusedAssets();
    }

    private void UnloadAllAssetBundles(bool unloadAllObjects)
    {
        AssetBundle.UnloadAllAssetBundles(unloadAllObjects);
    }
    #endregion

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