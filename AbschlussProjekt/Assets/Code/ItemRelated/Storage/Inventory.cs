using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : StorageSystem
{
    private List<EquipmentContainer> equippedStuff;
    private int currency;

    public Inventory(int storageSize) : base(storageSize)
    {
        equippedStuff = new List<EquipmentContainer>();
        currency = 0;
    }

}
