using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Path Container", menuName = "Data Container/Paths")]
public class Paths : DataContainer
{
	[Header("Paths")]
	public string SavefilePath = "/savefile.sfl";
	[SerializeField] private string assetBundlePath = "AssetBundles/StandaloneWindows/";
	public string AssetBundlePath
	{
		//get { return Path.Combine(Application.streamingAssetsPath, assetBundlePath); }
		get { return Application.streamingAssetsPath + "/"; }
	}

	[Space(10)]
	public string AreasPartialPath = "areas/";

	[Space(10)]
	public string PlayableCharactersPath = "characters/playable characters";
	public string VitalAssetsPath = "vital assets";
	public string UiPrefabsPath = "ui prefabs";
	public string ItemsPath = "items";
    public string ItemPoolsPath = "items/pools";
	public string SettingsPath = "settings";
	public string SkillsPath = "skills";

	[Header("Area Names")]
	public string DefaultLocation = "starter city";
	public string MainMenu = "main menu";
	public string FirstDungeon = "first dungeon";
	public string StandardCombatArea = "standard combat area";
	public string BossCombatArea = "boss combat area";
	public string CampSite = "camp site";
	public string Disco = "disco";
}
