using System;
using UnityEngine;

[Serializable] public class InventoryManager : Manager
{
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
		if (inventoryPanel.EquipItem(position))
		{
			Debug.Log("Item Equiped");
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

	public ItemContainer LoadItemContainer(string name)
	{
		return AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, name);
	}
}
