using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager
{
	public AreaData CurrentArea { get; private set; }
	public AreaData CurrentLoadedCombatArea { get; private set; }

	public GameManager()
	{
		var instance = AssetManager.Instance;

		//instance.CreateNewSavestate();
		instance.Load();

		CurrentArea = instance.LoadArea(instance.Paths.MainMenu);

		//Object.Instantiate(instance.LoadBundle<VitalAssets>(instance.Paths.VitalAssetsPath, "Vital Assets").MusicManagerAnchor);
	}

	public void UnloadAllAreas(AreaData newAreaToLoad)
	{
		UnloadCombatAreaAsync();
		LoadAreaAsync(newAreaToLoad);
	}
	public void LoadAreaAsync(AreaData areaToLoad)
	{
		//AssetManager.Instance.GetManager<InputManager>().RemoveAllListeners();
		//AssetManager.Instance.UnloadBundles();
		CurrentArea = areaToLoad;
		SceneManager.LoadSceneAsync(areaToLoad.Scene, LoadSceneMode.Single);

		AssetManager.Instance.GetManager<AudioManager>().FadeToNewPlaylist(CurrentArea.MusicPool);
		AssetManager.Instance.GetManager<LoadingScreenManager>().ShowLoadingScreen();
	}

	public void LoadCombatAreaAsync(AreaData combatAreaToLoad)
	{
		CurrentLoadedCombatArea = combatAreaToLoad;
		var asyncOp = SceneManager.LoadSceneAsync(CurrentLoadedCombatArea.Scene, LoadSceneMode.Additive);
		asyncOp.completed += CombatAreaLoadCompleted;

		AssetManager.Instance.GetManager<AudioManager>().FadeToNewPlaylist(CurrentLoadedCombatArea.MusicPool);
		AssetManager.Instance.GetManager<LoadingScreenManager>().ShowLoadingScreen();
	}

	private void CombatAreaLoadCompleted(AsyncOperation obj)
	{
		var objectsToDisable = SceneManager.GetSceneAt(0).GetRootGameObjects();
		for (int i = 0; i < objectsToDisable.Length; i++)
			objectsToDisable[i].SetActive(false);
		
		SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
	}

	public void UnloadCombatAreaAsync()
	{
		var asyncOp = SceneManager.UnloadSceneAsync(CurrentLoadedCombatArea.Scene);
		asyncOp.completed += CombatAreaUnloadCompleted;

		AssetManager.Instance.GetManager<AudioManager>().FadeToNewPlaylist(CurrentArea.MusicPool);
		AssetManager.Instance.GetManager<LoadingScreenManager>().ShowLoadingScreen();
	}

	private void CombatAreaUnloadCompleted(AsyncOperation obj)
	{
		CurrentLoadedCombatArea = null;

		var objectsToEnable = SceneManager.GetSceneAt(0).GetRootGameObjects();
		for (int i = 0; i < objectsToEnable.Length; i++)
			objectsToEnable[i].SetActive(true);

		SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
		AssetManager.Instance.GetManager<AudioManager>().SetNewPlaylist(CurrentArea.MusicPool);
	}

	public void ExitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void OnApplicationQuit()
	{
		Debug.Log("Saving");
		AssetManager.Instance.Save();
	}

	#region Debug
	public void StartCombatDebugging()
	{
		SaveDebugging();

		AssetManager instance = AssetManager.Instance;

		Character gunwoman = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Gunwoman");
		Character mage = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Mage");
		Character knight = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Knight");

		//return; // DEBUG

		CombatManager combatManager = instance.GetManager<CombatManager>() ?? new CombatManager();
		combatManager.StartCombat
			(instance.Savestate.CurrentTeam,
			new Entity[] {
				new Entity(knight),
				new Entity(mage),
				new Entity(gunwoman)
			});
	}

	private void SaveDebugging()
	{
		AssetManager instance = AssetManager.Instance;
		//instance.Load();
		instance.CreateNewSavestate();

		Character gunwoman = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Gunwoman");
		Character mage = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Mage");
		Character knight = instance.LoadBundle<Character>(instance.Paths.PlayableCharactersPath, "Knight");

		instance.Savestate.CurrentTeam = new Entity[] { new Entity(knight), new Entity(mage), new Entity(gunwoman) };
		instance.Savestate.Gold = 0;
		instance.Savestate.Souls = 0;
		instance.Savestate.OwnedCharacters = new List<Entity>(instance.Savestate.CurrentTeam);
	}
	#endregion
}