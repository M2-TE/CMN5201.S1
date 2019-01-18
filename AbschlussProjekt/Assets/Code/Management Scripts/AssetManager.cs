using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AssetManager
{
    private readonly string savefilePath = "/savefile.sfl";
    private readonly string assetBundlePath = "Assets/Asset Bundles/StandaloneWindows/";

	private readonly string itemsPath = "items";
    private readonly string settingsPath = "settings";
    private readonly string playableCharactersPath = "characters/playable characters";
    private readonly string equipmentPath = "equipment";
    private readonly string skillsPath = "skills";

	#region Getters/Setters
	private Camera mainCam;
	public Camera MainCam
	{
		get { return mainCam ?? (mainCam = Camera.main); }
	}

	private AssetBundle items;
	public AssetBundle Items
	{
		get { return items ?? (items = AssetBundle.LoadFromFile(assetBundlePath + itemsPath)); }
		set { if (value == null) items.Unload(true); }

	}

    private AssetBundle settings;
    public AssetBundle Settings
    {
        get { return settings ?? (settings = AssetBundle.LoadFromFile(assetBundlePath + settingsPath)); }
        set { if (value == null) settings.Unload(true); }
    }

    private AssetBundle playableCharacters;
    public AssetBundle PlayableCharacters
    {
        get { return playableCharacters ?? (playableCharacters = AssetBundle.LoadFromFile(assetBundlePath + playableCharactersPath)); }
        set { if (value == null) playableCharacters.Unload(true); }
    }

    private AssetBundle equipment;
    public AssetBundle Equipment
    {
        get { return equipment ?? (equipment = AssetBundle.LoadFromFile(assetBundlePath + equipmentPath)); }

        set { if (value == null) equipment.Unload(true); }
    }

    private AssetBundle skills;
    public AssetBundle Skills
    {
        get { return skills ?? (skills = AssetBundle.LoadFromFile(assetBundlePath + skillsPath)); }
        set { if (value == null) skills.Unload(true); }
    }

	private AssetBundle area;
	public AssetBundle Area
	{
		get { return null; }
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