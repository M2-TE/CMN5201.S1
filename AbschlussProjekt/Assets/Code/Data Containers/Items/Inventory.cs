using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Data Container/Storage/Inventory")]
public class Inventory : StorageSystem
{
    public int Currency;
    [HideInInspector] public int CurrentSelectedEntityInt;

    public Entity CurrentSelectedEntity
    {
        get
        {
            return AssetManager.Instance.Savestate.CurrentTeam[CurrentSelectedEntityInt];
        }
    }

    //public bool TryUnequipItem(EquipmentSlot type, out EquipmentContainer previousItem)
    //{
    //    previousItem = null;
    //    if (equippedItems.ContainsKey(type) && equippedItems[type].itemName != null && equippedItems[type].itemName != "")
    //    {
    //        previousItem = (EquipmentContainer)(AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, equippedItems[type].itemName));
    //        equippedItems[type].SetEmptySprite();
    //        return true;
    //    }
    //    return false;
    //}
}
