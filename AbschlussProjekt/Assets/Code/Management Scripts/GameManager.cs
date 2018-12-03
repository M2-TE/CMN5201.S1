using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private Savefile savefile;
    private CombatManager combatManager;
    private Entity[] playerTeam;

    private void Start ()
    {
        combatManager = GetComponent<CombatManager>();

        //savefile = AssetManager.Instance.Load();
        //playerTeam = savefile.CurrentTeam;





        //SAVE DEBUGGING
        Savefile savefile = new Savefile();
        Character gunwoman = AssetManager.Instance.Characters.LoadAsset<Character>("Gunwoman");
        Entity testEntity = new Entity(gunwoman);
        testEntity.currentHealth = 5;
        savefile.CurrentTeam = new Entity[] { new Entity(gunwoman), testEntity, new Entity(gunwoman), new Entity(gunwoman) };
        savefile.Gold = 42;
        savefile.Souls = 420;
        savefile.OwnedCharacters = new List<Entity>(savefile.CurrentTeam);
        savefile.Test = 50;
        AssetManager.Instance.Save(savefile);

        // LOAD DEBUGGING
        savefile = AssetManager.Instance.Load();
        Debug.Log(savefile.Gold + " " + savefile.Souls + " " + savefile.Test);

        Character[] chars = AssetManager.Instance.Characters.LoadAllAssets<Character>();
        foreach (Entity e in savefile.OwnedCharacters)
        {
            Debug.Log(e.Start().CharDataContainer.name);
        }
    }
}