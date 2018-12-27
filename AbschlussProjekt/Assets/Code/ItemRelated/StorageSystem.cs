using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{
    #region Variables & Properties
    private List<StorageSlot> storageSlots;
    private int storageColumns;
    private int storageAmount;

    public List<StorageSlot> StorageSlots { get { return storageSlots; } }
    public int StorageColumns {  get { return storageColumns;  } }
    public int StorageRows { get { return (int)Mathf.Ceil(storageColumns-1 / storageAmount)+1; } }
    public int StorageAmount { get { return storageAmount;  } }
    public int LastColumnStorage { get { return storageAmount % storageColumns; } }
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
        if (posX >= storageColumns || posY >= StorageRows || (posY == StorageRows-1 && posX >= LastColumnStorage))
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
        int tempRowCount = StorageRows;
        storageAmount += amount;
        int x = LastColumnStorage;
        for (int y = tempRowCount-1; y < StorageRows; y++)
        {
            for (; x < StorageColumns; x++)
            {
                storageSlots.Add(new StorageSlot(x, y));
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

    public bool ExtendStorageColumns(int amount)
    {
        storageAmount += amount;
        storageColumns += amount;
        for (int y = 0; y < StorageRows; y++)
        {
            for (int x = storageColumns-amount; x < StorageColumns; x++)
            {
                storageSlots.Insert(,)
            }
        }

        return true;
    }

    #endregion

}
