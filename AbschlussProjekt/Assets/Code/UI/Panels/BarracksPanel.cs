using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksPanel : UIPanel
{
	[Header("Barracks")]
	public GameObject StorageSlotPrefab;
	public GameObject DraggableSlotPrefab;
	public Transform CharStorageNode;
	public Vector2 CharStorageStartPos;
	public Vector2 CharStoragePerSlotOffset;

	private BarracksManager barracksManager;

	protected override void Awake()
	{
		base.Awake();
		barracksManager = AssetManager.Instance.GetManager<BarracksManager>() ?? new BarracksManager();
		barracksManager.RegisterPanel(this);
	}

	private void Start()
	{
		barracksManager.Initialize();
		ToggleVisibility(true);
	}
}
