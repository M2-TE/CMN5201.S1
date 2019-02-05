using UnityEngine;

public class EquippedCharacterPanel : UIPanel
{
	public CharacterSlot[] equippedCharSlots = new CharacterSlot[4];

	private GeneralCityManager manager;

	protected override void Awake()
	{
		base.Awake();
		manager = AssetManager.Instance.GetManager<GeneralCityManager>() ?? new GeneralCityManager();
	}

	private void Start()
	{
		manager.SetupEquippedCharPanel(this);
	}
}
