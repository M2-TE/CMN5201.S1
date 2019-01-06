using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlots { PRIMARY, SECONDARY, HEAD, CHEST, WAIST, HANDS, FEET, FINGER, NECK}

public class Inventory : StorageSystem
{
    private Dictionary<EquipmentSlots, EquipmentContainer> equippedStuff;

    private int currency;

    public Inventory(int storageSize) : base(storageSize)
    {
        equippedStuff = new Dictionary<EquipmentSlots, EquipmentContainer>();
        currency = 0;
    }

}
