using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemStorage", menuName = "Data Container/ItemStorage")]
public class StorageSystem : DataContainer
{
    #region Fields & Properties
    [SerializeField] public int Size = 0;
    [SerializeField] public List<StorageSlot> StorageSlots = new List<StorageSlot>();
    [SerializeField] public Dictionary<EquipmentSlot, UIElementHandler> equipedItems = new Dictionary<EquipmentSlot, UIElementHandler>();
    #endregion

    public bool TryEquipItem(EquipmentContainer item, out EquipmentContainer previousItem)
    {
        if (TryUnequipItem(item.EquipmentType, out previousItem) || equipedItems.ContainsKey(item.EquipmentType))
        {
            equipedItems[item.EquipmentType].itemName = item.ItemName;
            equipedItems[item.EquipmentType].Icon.sprite = item.ItemIcon;
            return true;
        }
        return false;
    }

    public bool TryUnequipItem(EquipmentSlot type, out EquipmentContainer previousItem)
    {
        previousItem = null;
        if (equipedItems.ContainsKey(type) && equipedItems[type].itemName != null && equipedItems[type].itemName != "")
        {
            previousItem = (EquipmentContainer)(AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, equipedItems[type].itemName));
            equipedItems[type].SetEmptySprite();
            return true;
        }
        return false;
    }
}
