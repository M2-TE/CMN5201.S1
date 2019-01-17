using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private Savestate savefile;
    private CombatManager combatManager;
    private Entity[] playerTeam;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void Start ()
	{
		StartCombatDebugging();
	}

	public void StartCombatDebugging()
	{
		combatManager = GetComponent<CombatManager>();

		SaveDebugging(/* remove this later */);
		LoadCurrentTeam();

		Character knight = AssetManager.Instance.Characters.LoadAsset<Character>("Knight");
		Character mage = AssetManager.Instance.Characters.LoadAsset<Character>("Mage");
		Character gunwoman = AssetManager.Instance.Characters.LoadAsset<Character>("Gunwoman");

		//return; // DEBUG
		combatManager.StartCombat
			(playerTeam,
			new Entity[] {
				new Entity(knight),
				new Entity(mage),
				new Entity(gunwoman)
			});
	}

    private void SaveDebugging()
    {
        Savestate savefile = new Savestate();
        Character gunwoman = AssetManager.Instance.Characters.LoadAsset<Character>("Gunwoman");
        Character mage = AssetManager.Instance.Characters.LoadAsset<Character>("Mage");
        Character knight = AssetManager.Instance.Characters.LoadAsset<Character>("Knight");
        savefile.CurrentTeam = new Entity[] { new Entity(knight), new Entity(mage), new Entity(gunwoman) };
        savefile.Gold = 0;
        savefile.Souls = 0;
        savefile.OwnedCharacters = new List<Entity>(savefile.CurrentTeam);

		AssetManager.Instance.Save(savefile);
    }

    private void LoadCurrentTeam()
    {
        savefile = AssetManager.Instance.Load();
        playerTeam = savefile.CurrentTeam;
    }
}