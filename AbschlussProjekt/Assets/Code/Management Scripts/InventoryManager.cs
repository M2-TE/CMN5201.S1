using UnityEngine;

public class InventoryManager : Manager
{
	private InventoryPanel panel;

	public void RegisterPanel(InventoryPanel invPanel)
	{
		panel = invPanel;
	}

	public void PickUpItem(int amount, ItemContainer container)
	{
		Debug.Log("Pick Up Item");
		panel.AddItemToInventory(amount, container);
	}

	public void EquipItem(int position)
	{
		Debug.Log("Equip Item");
		if (panel.EquipItem(position))
		{
			Debug.Log("Equiping was a success");
		}
		else
			Debug.Log("FAIL");
	}

	public void UnEquipItem(EquipmentSlot equipmentSlot)
	{
		Debug.Log("UnEquip Item");
	}

	public void ConsumeItem(int position)
	{
		Debug.Log("Consume Item");
	}

	public void DropItem(int position)
	{
		Debug.Log("Drop Item");
	}

	public void DisplayInformation(bool display)
	{
		Debug.Log("Display Information");
	}

	public ItemContainer LoadItemContainer(string name)
	{
		return AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, name);
	}
}
