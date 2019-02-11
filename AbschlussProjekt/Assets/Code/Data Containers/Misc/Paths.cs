using UnityEngine;

[CreateAssetMenu(fileName = "New Path Container", menuName = "Data Container/Paths")]
public class Paths : DataContainer
{
	[Header("Paths")]
	public string SavefilePath = "/savefile.sfl";
	public string AssetBundlePath = "AssetBundles/StandaloneWindows/";

	[Space(10)]
	public string AreasPartialPath = "areas/";

	[Space(10)]
	public string PlayableCharactersPath = "characters/playable characters";
	public string VitalAssetsPath = "vital assets";
	public string UiPrefabsPath = "ui prefabs";
	public string ItemsPath = "items";
	public string SettingsPath = "settings";
	public string EquipmentPath = "equipment";
	public string SkillsPath = "skills";

	[Header("Area Names")]
	public string DefaultLocation = "Starter City";
	public string MainMenu = "Main Menu";
	public string FirstDungeon = "First Dungeon";
	public string StandardCombatArea = "Standard Combat Area";
	public string BossCombatArea = "Boss Combat Area";
}
