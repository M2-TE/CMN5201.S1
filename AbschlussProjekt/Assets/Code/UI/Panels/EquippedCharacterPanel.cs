using UnityEngine;

public class EquippedCharacterPanel : UIPanel
{
	public CharacterSlot[] equippedCharSlots = new CharacterSlot[4];

	private GeneralCityManager manager;

	protected override void Awake()
	{
		base.Awake();

		// DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG
		AssetManager.Instance.CreateNewSavestate(); // DEBUG
		AssetManager.Instance.Load(); // DEBUG DEBUG DEBUG
		// DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG

		manager = AssetManager.Instance.GetManager<GeneralCityManager>() ?? new GeneralCityManager();
	}

	private void Start()
	{
		manager.SetupEquippedCharPanel(this);
	}
}
