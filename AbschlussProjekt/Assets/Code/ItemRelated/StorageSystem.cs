using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{
    #region Fields & Properties
    private List<StorageSlot> storageSlots;
    private int storageColumns;
    private int storageAmount;

    public List<StorageSlot> StorageSlots { get { return storageSlots; } }
    public int StorageColumns {  get { return storageColumns;  } }
    public int StorageRows { get { return (int)Mathf.Ceil(storageColumns-1 / storageAmount)+1; } }
    public int StorageAmount { get { return storageAmount;  } }
    public int LastRowStorageAmount { get { return storageAmount % storageColumns; } }
    #endregion

    #region Contructor
    public StorageSystem(int columns, int storageAmount)
    {
        InitializeList();
        storageColumns = columns;
        this.storageAmount = storageAmount;
    }
    #endregion

    #region StorageManagement
    private void InitializeList()
    {
        storageSlots = new List<StorageSlot>();
        for (int y = 0; y < storageAmount;)
        {
            for (int x = 0; x < storageColumns; x++)
            {
                storageSlots.Add(new StorageSlot(y, x));
                y++;
            }
        }
    }

    public StorageSlot GetSlotByPosition(int posX, int posY)
    {
        posX = (int)Mathf.Abs(posX);
        posY = (int)Mathf.Abs(posY);
        if (posX >= storageColumns || posY >= StorageRows || (posY == StorageRows-1 && posX >= LastRowStorageAmount))
            return null;
        else
            return storageSlots[posX - 1 + (posY-1)*StorageColumns];
    }

    public StorageSlot GetSlotByPosition(Vector2 position)
    {
        return GetSlotByPosition((int)position.x, (int)position.y);
    }

    public bool ExtendStorageAmount(int amount)
    {
        List<StorageSlot> extension = new List<StorageSlot>();
        for (int iterator = 0; iterator < amount; iterator++)
        {
            extension[iterator] = new StorageSlot(0, 0);
        }
        return ExtendStorageAmount(extension);
    }

    public bool ExtendStorageAmount(List<StorageSlot> extension)
    {
        int tempRowCount = StorageRows;
        storageAmount += extension.Count;
        int x = LastRowStorageAmount;
        int count = 0;
        for (int y = tempRowCount - 1; y < StorageRows; y++)
        {
            for (; x < StorageColumns; x++)
            {
                storageSlots.Add(extension[count].ChangeToPosition(x, y));
                count++;
            }
            x = 0;
        }
        return true;
    }

    public bool ReduceStorageAmount(int amount, out List<StorageSlot>removedSlots)
    {
        removedSlots = new List<StorageSlot>();
        if (StorageAmount - amount <= 1)
            return false;

        for (int count = 0; count < amount; count++)
        {
            removedSlots.Add(storageSlots[storageSlots.Count - 1]);
            storageSlots.RemoveAt(storageSlots.Count - 1);
        }
        storageAmount -= amount;
        return true;
    }

    public bool TryStoreItemAtNextFreeSlot(Item item, out Vector2 position)
    {
        position = new Vector2(-1,-1);
        for (int i = 0; i < StorageAmount; i++)
        {
            if(storageSlots[i].IsEmpty)
            {
                storageSlots[i].SwitchItems(item);
                position = storageSlots[i].Position;
                return true;
            }
        }
        return false;
    }
    #endregion

}
