using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AssetManager
{
	public Savestate Savestate;
	public List<Manager> ActiveManagers;
	private Dictionary<string, AssetBundle> loadedAssetBundles;

	private Paths paths;
	public Paths Paths
	{
		get { return paths ?? (paths = AssetBundle.LoadFromFile("AssetBundles/StandaloneWindows/paths").LoadAsset<Paths>("Paths")); }
	}
	
	public AreaData LoadArea(string sceneName)
	{
		return LoadBundle<AreaData>(Paths.AreasPartialPath + sceneName, sceneName);
	}

	public AssetType LoadBundle<AssetType>(string bundlePath, string assetName) where AssetType : DataContainer
	{
		AssetBundle bundle = null;
		if (loadedAssetBundles.ContainsKey(bundlePath)) bundle = loadedAssetBundles[bundlePath];
		else
		{
			bundle = AssetBundle.LoadFromFile(Paths.AssetBundlePath + bundlePath);
			loadedAssetBundles.Add(bundlePath, bundle);
		}
		return bundle.LoadAsset<AssetType>(assetName);
	}

	public void UnloadBundles(bool forceUnload = true)
	{
		foreach(AssetBundle bundle in loadedAssetBundles.Values)
		{
			bundle.Unload(forceUnload);
		}
		loadedAssetBundles.Clear();
	}

	public T GetManager<T>() where T : Manager
	{
		// return first match
		try { return ActiveManagers.OfType<T>().ToArray()[0]; }
		catch { return null; }
		
	}

	public void PrintAllManagers()
	{
		for (int i = 0; i < ActiveManagers.Count; i++)
			Debug.Log(ActiveManagers[i]);
	}

	public void CreateNewSavestate()
	{
		Savestate = new Savestate();
		var gunwoman = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Gunwoman");
		var knight = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Knight");
		var mage = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Mage");
		var priest = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Priest");
		var robot = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Robot");
		var wolf = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Wolf");
		var skeleton = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Skeleton");
		var skeletonArcher = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Skeleton Archer");
		var skeletonChief = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Skeleton Chief");

		Savestate.CurrentTeam[0] = new Entity(knight);
		Savestate.CurrentTeam[1] = new Entity(mage);
		Savestate.CurrentTeam[2] = new Entity(gunwoman);
		Savestate.CurrentTeam[3] = null;

		//Savestate.OwnedCharacters.Add(new Entity(gunwoman));
		//Savestate.OwnedCharacters.Add(new Entity(knight));
		//Savestate.OwnedCharacters.Add(new Entity(mage));
		Savestate.OwnedCharacters.Add(new Entity(priest));
		Savestate.OwnedCharacters.Add(new Entity(robot));
		Savestate.OwnedCharacters.Add(new Entity(wolf));
		Savestate.OwnedCharacters.Add(new Entity(skeleton));
		Savestate.OwnedCharacters.Add(new Entity(skeletonArcher));
		Savestate.OwnedCharacters.Add(new Entity(skeletonChief));

		Save();
	}

    public void Save()
    {
		StripAllCombatEffects();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + Paths.SavefilePath;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, Savestate);
        stream.Close();
    }

    public void Load()
    {
        string path = Application.persistentDataPath + Paths.SavefilePath;
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);
			Savestate = formatter.Deserialize(stream) as Savestate;
			stream.Close();
		}
		else
			CreateNewSavestate();
	}

	public void StripAllCombatEffects()
	{
		for(int i = 0; i < Savestate.CurrentTeam.Length; i++)
		{
			// CODE
		}

		for(int i = 0; i < Savestate.OwnedCharacters.Count; i++)
		{
			// CODE
		}
	}

    #region Singleton Implementation
    private static AssetManager instance;
    public static AssetManager Instance
    { get { return instance ?? (instance = new AssetManager()); } }
    private AssetManager()
    {
		ActiveManagers = new List<Manager>();
		loadedAssetBundles = new Dictionary<string, AssetBundle>();
	}
    #endregion
}