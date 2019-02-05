using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPanel : UIPanel
{
	public GameObject MapParent;

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
