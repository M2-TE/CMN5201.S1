using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPanel : UIPanel
{
	public GameObject MapParent;
	public int DungeonLength;
	public int MaxSiblings;

	[Header("Prefabs")]
	public GameObject EmptyNodePrefab;
	public GameObject CampfireNodePrefab;
	public GameObject CombatNodePrefab;
	public GameObject UnknownNodePrefab;

	private DungeonManager manager;

	protected override void Awake()
	{
		base.Awake();
		manager = AssetManager.Instance.GetManager<DungeonManager>() ?? new DungeonManager();
	}

	private void Start()
	{
		manager.CreateNewDungeon(this);
	}
}
