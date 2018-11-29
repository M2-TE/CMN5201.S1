using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [SerializeField] private string miscSettingsPath;

    private void Start()
    {
        //AssetBundle.LoadFromFile("PATH");
        AssetBundle.UnloadAllAssetBundles(true);
    }

    public static void Unload()
    {
        Resources.UnloadUnusedAssets();
    }
    #region Singleton Implementation
    private static AssetManager instance;
    public static AssetManager Instance
    { get { return (instance != null) ? instance : instance = new AssetManager(); } }
    private AssetManager() { }
    #endregion
}
