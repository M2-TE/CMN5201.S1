using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot { PRIMARY, SECONDARY, HEAD, CHEST, WAIST, HANDS, FEET, FINGERONE, FINGERTWO, NECK}

public class Inventory : MonoBehaviour
{
    [SerializeField] private Dictionary<EquipmentSlot, string> equippedStuff = new Dictionary<EquipmentSlot, string>();

    [SerializeField] private int currency = 0;

    [SerializeField] public StorageSystem InventoryContainer;

    [SerializeField] public GameObject SlotPrefab;

    private void Start()
    {
        InstantiateInventory();
    }

    private void InstantiateInventory()
    {
        for (int slot = 0; slot < InventoryContainer.Size; slot++)
        {
            InventoryContainer.StorageSlots.Add(new StorageSlot
            {
                Position = slot,
                Slot = GameObject.Instantiate(SlotPrefab, transform).GetComponent<SlotHolder>()
            });
        }
    }

    public bool AddItemToInventory(int stackAmount, ItemContainer container)
    {
        if(stackAmount > 0 && container != null)
        {
            InventoryContainer.StorageSlots[0].Amount = stackAmount;
            InventoryContainer.StorageSlots[0].Content = container.ItemName;
            return true;
        }
        return false;
    }
}
