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

		#region DEBUG
		ToggleVisibility(true);
		AssetManager.Instance.Load();
		#endregion

		barracksManager = AssetManager.Instance.GetManager<BarracksManager>() ?? new BarracksManager();
		barracksManager.RegisterPanel(this);
	}
}
