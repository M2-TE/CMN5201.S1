using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : Manager
{
    public ChestPanel ChestPanel;

    public InventoryManager InventoryManager;

    public void PrimaryActionOnItem(int position)
    {
        StorageSlot item = RemoveItem(position);
        if (item == null)
            return;
        if (InventoryManager != null)
            InventoryManager.TakeItem(item.amount, item.content);
        else
            AssetManager.Instance.Savestate.Inventory.Add(new StorageSlot(item.amount, item.content));
    }

    private StorageSlot RemoveItem(int position)
    {
        if (position >= ChestPanel.Chest.Items.Count || ChestPanel.Chest.Items[position] == null)
            return null;
        StorageSlot outSlot = ChestPanel.Chest.Items[position];
        ChestPanel.Chest.Items.RemoveAt(position);
        ChestPanel.DisplayChestItems(false);
        return outSlot;
    }
}
