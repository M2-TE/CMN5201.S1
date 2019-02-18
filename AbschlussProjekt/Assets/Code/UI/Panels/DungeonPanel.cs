using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPanel : UIPanel
{
	public GameObject MapParent;
	public DungeonNode.RoomType[] firstHalfRoomPool;
	public DungeonNode.RoomType[] secondHalfRoomPool;
	public int DungeonLength;
	public int MaxSiblings;
	public float randomPathingChance;
	public Color CurrentStandingColor = new Color(.5f, 0f, 0f, 1f);

	[Header("Prefabs")]
	public GameObject LineRendererPrefab;
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
