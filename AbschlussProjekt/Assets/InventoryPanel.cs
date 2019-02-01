using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public enum EquipmentSlot { PRIMARY, SECONDARY, HEAD, CHEST, WAIST, HANDS, FEET, FINGERONE, FINGERTWO, NECK }

public class InventoryPanel : UIPanel
{
	[SerializeField] private Dictionary<EquipmentSlot, string> equippedStuff = new Dictionary<EquipmentSlot, string>();
	[SerializeField] private int currency = 0;
	[SerializeField] public StorageSystem InventoryContainer;
	[SerializeField] public GameObject SlotPrefab;
	[SerializeField] private GameObject inventorySlotParent;

	private InventoryManager inventoryManager;

	protected override void Awake()
	{
		base.Awake();
		inventoryManager = AssetManager.Instance.GetManager<InventoryManager>() ?? new InventoryManager();
		inventoryManager.RegisterPanel(this);
	}

	private void Start()
	{
		InputManager manager = AssetManager.Instance.GetManager<InputManager>();
		void callback(InputAction.CallbackContext _) => ToggleVisibility();
		manager.AddListener(manager.Input.UI.InventoryOpen, callback);

		InstantiateInventory();
	}

	private void InstantiateInventory()
	{
		for (int slot = 0; slot < InventoryContainer.Size; slot++)
		{
			InventoryContainer.StorageSlots.Add(new StorageSlot
			{
				Position = slot,
				Slot = Instantiate(SlotPrefab, inventorySlotParent.transform).GetComponent<UIElementHandler>()
			});
		}
	}

	public bool AddItemToInventory(int stackAmount, ItemContainer container)
	{
		if (stackAmount > 0 && container != null)
		{
			for (int position = 0; position < InventoryContainer.StorageSlots.Count; position++)
			{
				StorageSlot tempSlot = InventoryContainer.StorageSlots[position];
				if (container.ItemName.Equals(tempSlot.Content) && container.StackingLimit >= (stackAmount + tempSlot.Amount))
				{
					tempSlot.Amount += stackAmount;
					return true;
				}
				else if (InventoryContainer.StorageSlots[position].Amount == 0)
				{
					tempSlot.Amount = stackAmount;
					tempSlot.Content = container.ItemName;
					return true;
				}
			}
		}
		return false;
	}

	public bool EquipItem(int position)
	{
		StorageSlot tempSlot = InventoryContainer.StorageSlots[position];
		if (tempSlot.Amount != 0)
		{
			LoadItemContainer(InventoryContainer.StorageSlots[position].Content);
			// If Equippable Equip dat shit :)
		}
		return false;
	}

	private ItemContainer LoadItemContainer(string name)
	{
		return AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, name);
	}
}
