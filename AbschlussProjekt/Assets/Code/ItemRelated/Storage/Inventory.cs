using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot { PRIMARY, SECONDARY, HEAD, CHEST, WAIST, HANDS, FEET, FINGERONE, FINGERTWO, NECK}

public class Inventory : MonoBehaviour
{
    [SerializeField] private Dictionary<EquipmentSlot, string> equippedStuff = new Dictionary<EquipmentSlot, string>();

    [SerializeField] private int currency = 0;

    [SerializeField] public StorageSystem InventoryContainer;
}
