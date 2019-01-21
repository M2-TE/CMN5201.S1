using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GameManager : MonoBehaviour, IManager
{
	public delegate void ExtendedUpdate();
	public List<ExtendedUpdate> extendedUpdates;

	private Savestate savestate;
	private AudioSource musicPlayer;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(AssetManager.Instance.MainCanvas);
		AssetManager.Instance.GameManager = this;
		extendedUpdates = new List<ExtendedUpdate>();
		
		AssetManager.Instance.MainMenuManager.SetActive(true);
		//StartCombatDebugging();
	}

	private void Update()
	{
		for(int i = 0; i < extendedUpdates.Count; i++)
			extendedUpdates[i]();
	}

	#region Debug
	public void StartCombatDebugging()
	{
		SaveDebugging();
		LoadGame();

		Character knight = AssetManager.Instance.PlayableCharacters.LoadAsset<Character>("Knight");
		Character mage = AssetManager.Instance.PlayableCharacters.LoadAsset<Character>("Mage");
		Character gunwoman = AssetManager.Instance.PlayableCharacters.LoadAsset<Character>("Gunwoman");

		//return; // DEBUG

		CombatManager combatManager = AssetManager.Instance.CombatManager;
		combatManager.StartCombat
			(savestate.CurrentTeam,
			new Entity[] {
				new Entity(knight),
				new Entity(mage),
				new Entity(gunwoman)
			});
	}

    private void SaveDebugging()
    {
        Savestate savefile = new Savestate();
        Character gunwoman = AssetManager.Instance.PlayableCharacters.LoadAsset<Character>("Gunwoman");
        Character mage = AssetManager.Instance.PlayableCharacters.LoadAsset<Character>("Mage");
        Character knight = AssetManager.Instance.PlayableCharacters.LoadAsset<Character>("Knight");
        savefile.CurrentTeam = new Entity[] { new Entity(knight), new Entity(mage), new Entity(gunwoman) };
        savefile.Gold = 0;
        savefile.Souls = 0;
        savefile.OwnedCharacters = new List<Entity>(savefile.CurrentTeam);

		AssetManager.Instance.Save(savefile);
    }
	#endregion

	public void CreateNewSavestate()
	{
		savestate = new Savestate();
	}

	public void SaveGame()
	{
		AssetManager.Instance.Save(savestate);
	}

	public void LoadGame()
	{
		savestate = AssetManager.Instance.Load();
	}

	public void ExitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	private void OnApplicationQuit()
	{
		// stuff
	}
}