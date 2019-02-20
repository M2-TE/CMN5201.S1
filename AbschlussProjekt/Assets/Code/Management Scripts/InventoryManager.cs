using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Manager
{
    public InventoryPanel InventoryPanel;

    public CharacterInfoManager CharacterInfoManager;

    public bool OpenInvetoryPanel => InventoryPanel.Open;

    #region Actions

    public void PrimaryActionOnItem(int position)
    {
        if (LoadItem(position) == null)
            return;

        if (LoadItem(position).GetType().Equals(typeof(MiscContainer)))
        {
            ConsumeItem((MiscContainer)LoadItem(position));
        }
        else
        {
            if(EquipItem((EquipmentContainer) LoadItem(position)))
                RemoveItem(position);
        }
    }

    public void TakeItem(int amount, ItemContainer container)
	{
        AddItem(amount, container.ItemName);
	}

    public void TakeItem(int amount, string itemName)
    {
        AddItem(amount, itemName);
    }

    public bool EquipItem(EquipmentContainer item)
    {
        if (CharacterInfoManager == null || !CharacterInfoManager.OpenCharacterPanel || !item.CheckMatchingClass(CharacterInfoManager.CharacterInfoPanel.CurrentCharacter.CharacterType))
            return false;
        CharacterInfoManager.EquipItem(item);
        return true;
    }

	public void ConsumeItem(MiscContainer item)
	{
        if (item.IsUsable.Equals(Usability.ALWAYS_USABLE) || item.IsUsable.Equals(Usability.OUT_OF_COMBAT_ONLY))
        {
            // CONSUME ITEM
        }
    }

    private void AddItem(int amount, string itemName)
    {
        AssetManager.Instance.Savestate.Inventory.Add(new StorageSlot(amount, itemName));
        InventoryPanel.DisplayItemInfo(0, false);
        InventoryPanel.DisplayInventory();
    }

    private bool RemoveItem(int position)
    {
        if (position >= AssetManager.Instance.Savestate.Inventory.Count || AssetManager.Instance.Savestate.Inventory[position] == null)
            return false;
        
        AssetManager.Instance.Savestate.Inventory.RemoveAt(position);
        InventoryPanel.DisplayItemInfo(0, false);
        InventoryPanel.DisplayInventory();
        return true;
    }

    public ItemContainer LoadItem(int position)
    {
        if (position >= AssetManager.Instance.Savestate.Inventory.Count)
            return null;
        return LoadItem(AssetManager.Instance.Savestate.Inventory[position].content);
    }

    public ItemContainer LoadItem(string name)
	{
        if (name == "" || name == null)
            return null;
        return AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, name);
	}

    public void TryRemoveItem()
    {
        if(InventoryPanel.currentHover >= 0)
            RemoveItem(InventoryPanel.currentHover);
    }

    #endregion
}
