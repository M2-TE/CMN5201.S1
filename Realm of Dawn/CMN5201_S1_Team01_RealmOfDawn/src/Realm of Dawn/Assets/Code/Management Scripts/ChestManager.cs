using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : Manager
{
    public ChestPanel ChestPanel;

    public InventoryManager InventoryManager;

    public List<StorageSlot> Items;

    public void PrimaryActionOnItem(int position)
    {
        if (position >= Items.Count || Items[position] == null)
            return;
        if (InventoryManager != null)
            InventoryManager.TakeItem(Items[position].amount, Items[position].content);
        else
            AssetManager.Instance.Savestate.Inventory.Add(new StorageSlot(Items[position].amount, Items[position].content));
        RemoveItem(position);
    }

    private bool RemoveItem(int position)
    {
        if (position >= Items.Count || Items[position] == null)
            return false;
        Items.RemoveAt(position);
        ChestPanel.DisplayItemsInChest();
        return true;
    }
}
