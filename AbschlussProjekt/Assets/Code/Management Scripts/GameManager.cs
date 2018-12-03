using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private Savestate savefile;
    private CombatManager combatManager;
    private Entity[] playerTeam;

    private void Start ()
    {
        combatManager = GetComponent<CombatManager>();

        //savefile = AssetManager.Instance.Load();
        //playerTeam = savefile.CurrentTeam;





        LoadCurrentTeam();

        Character mage = AssetManager.Instance.Characters.LoadAsset<Character>("Mage");
        combatManager.StartCombat
            (playerTeam, 
            new Entity[] {
                new Entity(mage),
                new Entity(mage),
                null,
                null
            });
    }

    private void LoadCurrentTeam()
    {
        savefile = AssetManager.Instance.Load();
        savefile.InitializeTeam();
        playerTeam = savefile.CurrentTeam;
    }

    private void SaveDebugging()
    {
        Savestate savefile = new Savestate();
        Character gunwoman = AssetManager.Instance.Characters.LoadAsset<Character>("Gunwoman");
        Character mage = AssetManager.Instance.Characters.LoadAsset<Character>("Mage");
        Character knight = AssetManager.Instance.Characters.LoadAsset<Character>("Knight");
        savefile.CurrentTeam = new Entity[] { new Entity(knight), new Entity(gunwoman), new Entity(mage), null };
        savefile.Gold = 0;
        savefile.Souls = 0;
        savefile.OwnedCharacters = new List<Entity>(savefile.CurrentTeam);
        AssetManager.Instance.Save(savefile);
    }
}