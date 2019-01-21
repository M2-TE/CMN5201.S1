using UnityEngine;

[CreateAssetMenu(fileName = "New UI Prefab Container", menuName = "Data Container/UI Prefabs")]
public class UIPrefabs : DataContainer 
{
	public GameObject MainUICanvasPrefab;
	[Space()]
	public GameObject MainMenuUIPrefab;
	public GameObject CombatUIPrefab;
}
