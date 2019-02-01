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
                Slot = GameObject.Instantiate(SlotPrefab, transform).GetComponent<UIElementHandler>()
            });
        }
    }

    public bool AddItemToInventory(int stackAmount, ItemContainer container)
    {
        if (stackAmount > 0 && container != null)
        {
            for (int position = 0; position < InventoryContainer.StorageSlots.Count ; position++)
            {
                StorageSlot tempSlot = InventoryContainer.StorageSlots[position];
                if (container.ItemName.Equals(tempSlot.Content) && container.StackingLimit >= (stackAmount + tempSlot.Amount))
                {
                    tempSlot.Amount += stackAmount;
                    return true;
                }
                else if(InventoryContainer.StorageSlots[position].Amount == 0)
                {
                    tempSlot.Amount = stackAmount;
                    tempSlot.Content = container.ItemName;
                    return true;
                }
            }
        }
        return false;
    }

    public bool EquipItem(int position)
    {
        StorageSlot tempSlot = InventoryContainer.StorageSlots[position];
        if (tempSlot.Amount != 0)
        {
            LoadItemContainer(InventoryContainer.StorageSlots[position].Content);
            // If Equippable Equip dat shit :)
        }
        return false;
    }

    private ItemContainer LoadItemContainer(string name)
    {
        return AssetManager.Instance.Items.LoadAsset<ItemContainer>(name);
    }
}
