using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AssetManager : IManager
{
	// put these into a path data container \/
	private readonly string savefilePath = "/savefile.sfl";
    private readonly string assetBundlePath = "AssetBundles/StandaloneWindows/";

	private readonly string uiPrefabsPath = "ui prefabs";
	private readonly string itemsPath = "items";
    private readonly string settingsPath = "settings";
    private readonly string playableCharactersPath = "characters/playable characters";
    private readonly string equipmentPath = "equipment";
    private readonly string skillsPath = "skills";

	#region Getters/Setters
	public Savestate Savestate;

	#region Instances
	//// REWORK THIS \/ (fuck Camera.main, honestly)
	//private Camera mainCam;
	//public Camera MainCam
	//{
	//	get { return mainCam ?? (mainCam = Camera.main); }
	//}

	//private MainMenuManager mainMenuManager;
	//public MainMenuManager MainMenuManager
	//{
	//	get { return mainMenuManager ?? (mainMenuManager = new MainMenuManager()); }
	//}

	//private CombatManager combatManager;
	//public CombatManager CombatManager
	//{
	//	get { return combatManager ?? (combatManager = new CombatManager()); }
	//}

	//private GameObject mainCanvas;
	//public GameObject MainCanvas
	//{
	//	get { return mainCanvas ?? (mainCanvas = Object.Instantiate(UIPrefabs.LoadAsset<UIPrefabs>("UIPrefabs").MainUICanvasPrefab)); }
	//}
	#endregion

	private AssetBundle uiPrefabs;
	public AssetBundle UIPrefabs
	{
		get { return uiPrefabs ?? (uiPrefabs = AssetBundle.LoadFromFile(assetBundlePath + uiPrefabsPath)); }
		set { if (value == null) uiPrefabs.Unload(true); }
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

	// PROTOTYPE
	public AssetType LoadBundle<AssetType>(string path, string assetName) where AssetType : DataContainer
	{
		return AssetBundle.LoadFromFile(assetBundlePath + path).LoadAsset<AssetType>(assetName);
	}

	public void CreateNewSavestate()
	{
		Savestate = new Savestate();
	}

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + savefilePath;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, Savestate);
        stream.Close();
    }

    public void Load()
    {
        string path = Application.persistentDataPath + savefilePath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Savestate = formatter.Deserialize(stream) as Savestate;
            stream.Close();
        }
        else Debug.LogError("Savefile not found.");
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