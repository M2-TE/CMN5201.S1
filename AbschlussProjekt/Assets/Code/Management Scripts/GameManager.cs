#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif
using UnityEngine;

public class GameManager : Manager
{
	#region Debug
	public void StartCombatDebugging()
	{
		SaveDebugging();

		AssetManager instance = AssetManager.Instance;

		//Character knight = instance.PlayableCharacters.LoadAsset<Character>("Knight");
		//Character mage = instance.PlayableCharacters.LoadAsset<Character>("Mage");
		//Character gunwoman = instance.PlayableCharacters.LoadAsset<Character>("Gunwoman");
		Character gunwoman = instance.LoadBundle<Character>("characters/playable characters", "Gunwoman");
		Character mage = instance.LoadBundle<Character>("characters/playable characters", "Mage");
		Character knight = instance.LoadBundle<Character>("characters/playable characters", "Knight");

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

		//Character gunwoman = instance.PlayableCharacters.LoadAsset<Character>("Gunwoman");
		//Character mage = instance.PlayableCharacters.LoadAsset<Character>("Mage");
		//Character knight = instance.PlayableCharacters.LoadAsset<Character>("Knight");
		Character gunwoman = instance.LoadBundle<Character>("characters/playable characters", "Gunwoman");
		Character mage = instance.LoadBundle<Character>("characters/playable characters", "Mage");
		Character knight = instance.LoadBundle<Character>("characters/playable characters", "Knight");

		instance.Savestate.CurrentTeam = new Entity[] { new Entity(knight), new Entity(mage), new Entity(gunwoman) };
		instance.Savestate.Gold = 0;
		instance.Savestate.Souls = 0;
		instance.Savestate.OwnedCharacters = new List<Entity>(instance.Savestate.CurrentTeam);
	}
	#endregion

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
		// stuff
	}
}