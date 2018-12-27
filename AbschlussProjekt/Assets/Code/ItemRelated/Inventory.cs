using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : StorageSystem
{
    private Item[] equippedStuff;

    public Inventory(int columns, int storageAmount) : base(columns, storageAmount)
    {
        equippedStuff = new Item[10];
    }

    
}
