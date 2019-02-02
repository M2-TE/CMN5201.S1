using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class InventoryManager : Manager
{
    [SerializeField] private Dictionary<EquipmentSlot, UIElementHandler> equippedItems = new Dictionary<EquipmentSlot, UIElementHandler>();

    private InventoryPanel inventoryPanel;

    public InventoryPanel InventoryPanel
    {
        get
        {
            return inventoryPanel;
        }
        set
        {
            inventoryPanel = value;
        }
    }

    public void AddHandlerToInventory(EquipmentSlot key, UIElementHandler handler)
    {
        equippedItems.Add(key, handler);
    }

    public void PickUpItem(int amount, ItemContainer container)
	{
		Debug.Log("Pick Up Item...");
		if(inventoryPanel.AddItemToInventory(amount, container))
        {
            Debug.Log("Item was picked up");
        }
	}

	public void EquipItem(int position)
	{
		Debug.Log("Equip Item...");
		if (inventoryPanel.EquipItem(position, out EquipmentSlot slot))
		{
			Debug.Log("Item Equiped");
            UpdateCharacterEquipmentDisplay(inventoryPanel.InventoryContainer.CurrentSelectedEntity,slot);
            inventoryPanel.itemInfoPanel.CloseItemInfo();
        }
		else
			Debug.Log("Unable to Equip Item");
	}

	public void UnequipItem(EquipmentSlot slot)
	{
		Debug.Log("Unequip Item...");
        if (inventoryPanel.UnequipItem(slot))
        {
            Debug.Log("Item Unequiped");
            UpdateCharacterEquipmentDisplay(inventoryPanel.InventoryContainer.CurrentSelectedEntity, slot);
            inventoryPanel.itemInfoPanel.CloseItemInfo();
        }
        else
            Debug.Log("Unable to Unequip Item");
    }

	public void ConsumeItem(int position)
	{
		Debug.Log("Consume Item...");
        if (inventoryPanel.RemoveSingleItemFromInventory(position, out ItemContainer container))
        {
            Debug.Log("Item Removed (not consumed -> Warning: there is no reference to any character stats yet... unfortunetly)");
            inventoryPanel.itemInfoPanel.CloseItemInfo();
        }
    }

	public void DropItem(int position)
	{
		Debug.Log("Drop Item...");
        if(inventoryPanel.RemoveItemsFromInventory(position, out ItemContainer container, out int amount))
        {
            Debug.Log(amount +" Item(s) Removed (not dropped -> Warning: There is no parent to drop the item gameObject yet)");
            inventoryPanel.itemInfoPanel.CloseItemInfo();
        }
	}

	public void DisplayItemInformation(int position, bool display)
	{
        ItemContainer item = inventoryPanel.GetItemContainerAtPosition(position);
        if (display && item != null)
            inventoryPanel.itemInfoPanel.OpenItemInfo(item, ItemInfoType.STORAGE);
        else
            inventoryPanel.itemInfoPanel.CloseItemInfo();
	}

    public void DisplayItemInformation(EquipmentSlot slot, bool display)
    {
        ItemContainer item = inventoryPanel.InventoryContainer.CurrentSelectedEntity.GetEquippedItem(slot);
         if (display && item != null)
            inventoryPanel.itemInfoPanel.OpenItemInfo(item, ItemInfoType.EQUIPPED);
        else
            inventoryPanel.itemInfoPanel.CloseItemInfo();
    }

    public void OpenCharacterInformationDisplay(Entity character)
    {

        UpdateCharacterEquipmentDisplay(character);
    }

    public void UpdateCharacterEquipmentDisplay(Entity character)
    {
        foreach (EquipmentSlot slot in equippedItems.Keys)
        {
            UpdateCharacterEquipmentDisplay(character, slot);
        }
    }

    public void UpdateCharacterEquipmentDisplay(Entity character, EquipmentSlot slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            if(character.GetEquippedItem(slot) != null)
            {
                equippedItems[slot].itemName = character.GetEquippedItem(slot).ItemName;
                equippedItems[slot].Icon.sprite = character.GetEquippedItem(slot).ItemIcon;
            }
            else
            {
                equippedItems[slot].SetEmpty();
            }
        }
        else
            Debug.Log("An error has been occurred.");

    }

    public bool CheckForEquippedItemAtSlot(EquipmentSlot slot, out EquipmentContainer container)
    {
        container = null;
        if (equippedItems.ContainsKey(slot))
        {
            container = (EquipmentContainer)LoadItemContainer(equippedItems[slot].itemName);
            if (container != null)
                return true;
        }
        return false;
    }

	public ItemContainer LoadItemContainer(string name)
	{
        if (name != "" && name != null)
            return AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, name);
        else
            return null;
	}
}
