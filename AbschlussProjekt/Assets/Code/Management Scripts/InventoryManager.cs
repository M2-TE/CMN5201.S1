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
		}
		else
			Debug.Log("Unable to Equip Item");
	}

	public void UnequipItem(EquipmentSlot equipmentSlot)
	{
		Debug.Log("Unequip Item...");
        if (inventoryPanel.UnequipItem(equipmentSlot))
        {
            Debug.Log("Item Unequiped");
        }
	}

	public void ConsumeItem(int position)
	{
		Debug.Log("Consume Item...");
        if (inventoryPanel.RemoveSingleItemFromInventory(position, out ItemContainer container))
        {
            Debug.Log("Item Removed (not consumed -> Warning: there is no reference to any character stats yet... unfortunetly)");
        }
    }

	public void DropItem(int position)
	{
		Debug.Log("Drop Item...");
        if(inventoryPanel.RemoveItemsFromInventory(position, out ItemContainer container, out int amount))
        {
            Debug.Log(amount +" Item(s) Removed (not dropped -> Warning: There is no parent to drop the item gameObject yet)");
        }
	}

	public void DisplayInformation(bool display)
	{

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

	public ItemContainer LoadItemContainer(string name)
	{
		return AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, name);
	}
}
