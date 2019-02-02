using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public enum EquipmentSlot { PRIMARY, SECONDARY, HEAD, CHEST, WAIST, HANDS, FEET, FINGERONE, FINGERTWO, NECK }

public class InventoryPanel : UIPanel
{
	
	[SerializeField] private int currency = 0;
	[SerializeField] private StorageSystem inventoryContainer;
	[SerializeField] private GameObject slotPrefab;
	[SerializeField] private GameObject inventorySlotParent;
    [SerializeField] private GameObject itemPrefab;

	private InventoryManager inventoryManager;

	protected override void Awake()
	{
		base.Awake();
		inventoryManager = AssetManager.Instance.GetManager<InventoryManager>() ?? new InventoryManager();
        inventoryManager.InventoryPanel = this;
	}

	private void Start()
	{
		InputManager manager = AssetManager.Instance.GetManager<InputManager>();
		//void callback(InputAction.CallbackContext _) => ToggleVisibility();
		manager.AddListener(manager.Input.UI.InventoryOpen, ctx => ToggleVisibility());

		InstantiateInventory();
	}

    public void AddHandlerToInventory(EquipmentSlot key, UIElementHandler handler)
    {
        inventoryContainer.equipedItems.Add(key, handler);
    }

	private void InstantiateInventory()
	{
		for (int slot = 0; slot < inventoryContainer.Size; slot++)
		{
			inventoryContainer.StorageSlots.Add(new StorageSlot
			{
				Position = slot,
				Slot = Instantiate(slotPrefab, inventorySlotParent.transform).GetComponent<UIElementHandler>()
			});
		}
	}

	public bool AddItemToInventory(int stackAmount, ItemContainer container)
	{
		if (stackAmount > 0 && container != null)
		{
			for (int position = 0; position < inventoryContainer.StorageSlots.Count; position++)
			{
				StorageSlot tempSlot = inventoryContainer.StorageSlots[position];
				if (container.ItemName.Equals(tempSlot.Content) && container.StackingLimit >= (stackAmount + tempSlot.Amount))
				{
					tempSlot.Amount += stackAmount;
					return true;
				}
				else if (inventoryContainer.StorageSlots[position].Amount == 0)
				{
					tempSlot.Amount = stackAmount;
					tempSlot.Content = container.ItemName;
					return true;
				}
			}
		}
		return false;
	}

    public bool RemoveItemsFromInventory(int position, out ItemContainer container, out int amount)
    {
        StorageSlot tempSlot = inventoryContainer.StorageSlots[position];
        container = null;
        amount = tempSlot.Amount;
        if (tempSlot.Amount != 0)
        {
            container = LoadItemContainer(inventoryContainer.StorageSlots[position].Content);
            tempSlot.EmptySlot();
            return true;
        }
        return false;
    }

    public bool RemoveSingleItemFromInventory(int position, out ItemContainer container)
    {
        StorageSlot tempSlot = inventoryContainer.StorageSlots[position];
        container = null;
        if (tempSlot.Amount != 0)
        {
            container = LoadItemContainer(inventoryContainer.StorageSlots[position].Content);
            tempSlot.Amount -= 1;
            return true;
        }
        return false;
    }

	public bool EquipItem(int position)
	{
		StorageSlot tempSlot = inventoryContainer.StorageSlots[position];
		if (tempSlot.Amount != 0)
		{
			ItemContainer temp = LoadItemContainer(inventoryContainer.StorageSlots[position].Content);
            if (temp.GetItemType().Equals(ItemType.EQUIPMENT))
            {
                tempSlot.EmptySlot();
                inventoryContainer.TryEquipItem((EquipmentContainer)temp, out EquipmentContainer previousItem);
                AddItemToInventory(1, previousItem);
                return true;
            }
		}
		return false;
	}

    public bool UnequipItem(EquipmentSlot slot)
    {
        if (inventoryContainer.TryUnequipItem(slot, out EquipmentContainer item))
            return AddItemToInventory(1, item);
        return false;
    }

	private ItemContainer LoadItemContainer(string name)
	{
		return AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, name);
	}
}
