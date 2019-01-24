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
	public string VitalPrefabsPath = "vital prefabs";
	public string UiPrefabsPath = "ui prefabs";
	public string ItemsPath = "items";
	public string SettingsPath = "settings";
	public string EquipmentPath = "equipment";
	public string SkillsPath = "skills";

	[Header("Scene Names")]
	public string DefaultLocation = "Starter City";
}
