using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IManager
{
	private Savestate savefile;
    private Entity[] playerTeam;
	private AudioSource musicPlayer;

	private CombatManager combatManager;
	private MainMenuManager mainMenuManager;
	private bool combatActive;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(AssetManager.Instance.MainCanvas);
		AssetManager.Instance.GameManager = this;
	}

	private void Start()
	{
		mainMenuManager = new MainMenuManager();
		// StartCombatDebugging();
	}

	private void Update()
	{
		if (combatActive) combatManager.CheckForUserInput();
	}

	#region Debug
	public void StartCombatDebugging()
	{
		SaveDebugging(/* remove this later */);
		LoadCurrentTeam();

		Character knight = AssetManager.Instance.PlayableCharacters.LoadAsset<Character>("Knight");
		Character mage = AssetManager.Instance.PlayableCharacters.LoadAsset<Character>("Mage");
		Character gunwoman = AssetManager.Instance.PlayableCharacters.LoadAsset<Character>("Gunwoman");

		//return; // DEBUG

		combatManager = new CombatManager
			(playerTeam,
			new Entity[] {
				new Entity(knight),
				new Entity(mage),
				new Entity(gunwoman)
			});
		combatActive = true;
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

	private void SaveCurrentTeam()
	{
		AssetManager.Instance.Save(savefile);
	}

	private void LoadCurrentTeam()
    {
        savefile = AssetManager.Instance.Load();
        playerTeam = savefile.CurrentTeam;
    }

	public void ExitGame()
	{

	}
}