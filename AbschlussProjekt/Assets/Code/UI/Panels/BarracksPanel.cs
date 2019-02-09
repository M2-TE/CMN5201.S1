using System.Collections.Generic;
using UnityEngine;

public class BarracksPanel : UIPanel
{
	public DebugCityPanel debugCityPanel;
	public List<CharacterSlot> barrackSlots;
	public GameObject characterSlotPrefab;
	public GameObject draggableSlotPrefab;
	public RectTransform CharacterStorageNode;
	public Vector2 StartingSlotOffset;
	public Vector2 SlotOffset;

	private GeneralCityManager manager;

	protected override void Awake()
	{
		base.Awake();
		manager = AssetManager.Instance.GetManager<GeneralCityManager>() ?? new GeneralCityManager();
	}

	private void Start()
	{
		manager.SetupBarracksPanel(this);
	}

	public void OnBarracksExitButtonPress()
	{
		ToggleVisibility(false);
		debugCityPanel.ToggleVisibility(true);
	}
}
