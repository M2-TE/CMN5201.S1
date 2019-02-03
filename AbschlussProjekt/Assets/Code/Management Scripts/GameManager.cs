using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class GameManager : Manager
{
	public AreaData CurrentArea { get; private set; }

	public void LoadAreaAsync(AreaData areaToLoad)
	{
		//AssetManager.Instance.GetManager<InputManager>().RemoveAllListeners();
		AssetManager.Instance.UnloadBundles();

		CurrentArea = areaToLoad;
		SceneManager.LoadSceneAsync(areaToLoad.Scene, areaToLoad.LoadSceneMode);
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

		CombatManager combatManager = instance.GetManager<CombatManager>();
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
		instance.Load();

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