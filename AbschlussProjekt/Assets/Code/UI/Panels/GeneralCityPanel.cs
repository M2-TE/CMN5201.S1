using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCityPanel : UIPanel
{
	public GameObject DraggableSlotPrefab;
	public StoredCharacterSlot[] EquippedCharSlots = new StoredCharacterSlot[4];

	private GeneralCityManager manager;

	protected override void Awake()
	{
		// DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG
		AssetManager.Instance.Load();
		// DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG

		base.Awake();
		manager = AssetManager.Instance.GetManager<GeneralCityManager>() ?? new GeneralCityManager();
		manager.Register(this);
	}

	private void Start()
	{
		manager.Initialize();
		ToggleVisibility(true);
	}
}
