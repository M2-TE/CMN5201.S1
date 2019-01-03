using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{
    #region Fields & Properties
    private List<StorageSlot> storageSlots;
    private int storageSize;

    public List<StorageSlot> StorageSlots { get { return storageSlots; } }
    public int StorageSize { get { return storageSize;  } }
    #endregion

    #region Contructor
    public StorageSystem(int storageSize)
    {
        InitializeList();
        this.storageSize = storageSize;
    }
    #endregion

    #region StorageManagement
    private void InitializeList()
    {
        storageSlots = new List<StorageSlot>();
        for (int position = 0; position < storageSize; position++)
        {
            storageSlots.Add(new StorageSlot(position));
        }
    }

    public StorageSlot GetSlotByPosition(int position)
    {
        if (position >= storageSize)
            return null;
        else
            return storageSlots[position-1];
    }

    public bool ExtendStorageAmount(int amount)
    {
        List<StorageSlot> extension = new List<StorageSlot>();
        for (int iterator = 0; iterator < amount; iterator++)
        {
            extension[iterator] = new StorageSlot(-1);
        }
        return ExtendStorageAmount(extension);
    }

    public bool ExtendStorageAmount(List<StorageSlot> extension)
    {
        for (int size = 0; size < extension.Count; size++)
        {
            storageSlots.Add(extension[size].ChangePosition(storageSize + size));
        }
        storageSize += extension.Count;
        return true;
    }

    public bool ReduceStorageAmount(int amount, out List<StorageSlot>removedSlots)
    {
        removedSlots = new List<StorageSlot>();
        if (storageSize - amount <= 1)
            return false;

        for (int count = 0; count < amount; count++)
        {
            removedSlots.Add(storageSlots[storageSlots.Count - 1]);
            storageSlots.RemoveAt(storageSlots.Count - 1);
        }
        storageSize -= amount;
        return true;
    }

    public bool TryStoreItemAtNextFreeSlot(Item item, out int position)
    {
        position = -1;
        for (int i = 0; i < StorageSize; i++)
        {
            if(storageSlots[i].IsEmpty)
            {
                storageSlots[i].Content = item;
                position = storageSlots[i].Position;
                return true;
            }
        }
        return false;
    }
    #endregion

}
