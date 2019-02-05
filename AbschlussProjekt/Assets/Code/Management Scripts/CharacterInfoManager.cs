using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoManager : Manager
{
    public CharacterInfoPanel CharacterInfoPanel;

    public InventoryManager InventoryManager;

    public bool OpenCharacterPanel => CharacterInfoPanel.gameObject.activeInHierarchy;

    public EquipmentContainer GetItemOfCurrentCharacter(EquipmentSlot slot)
    {
        return CharacterInfoPanel.CurrentCharacter.GetEquippedItem(slot);
    }

    public void SetItemOfCurrentCharacter(EquipmentContainer item)
    {
        CharacterInfoPanel.CurrentCharacter.SetEquippedItem(item.EquipmentType, item);
    }

    public void PrimaryActionOnItem(EquipmentSlot slot)
    {
        if (GetItemOfCurrentCharacter(slot) == null)
            return;
        UnequipItem(slot);
    }

    public void UnequipItem(EquipmentSlot slot)
    {
        if (InventoryManager != null)
            InventoryManager.TakeItem(1, GetItemOfCurrentCharacter(slot));
        else
            AssetManager.Instance.Savestate.Inventory.Add(new StorageSlot(1, GetItemOfCurrentCharacter(slot).ItemName));
        CharacterInfoPanel.DisplayEquipmentSlot(slot);
    }

    public void EquipItem(EquipmentContainer item)
    {
        PrimaryActionOnItem(item.EquipmentType);
        SetItemOfCurrentCharacter(item);
        CharacterInfoPanel.DisplayEquipmentSlot(item.EquipmentType);
    }
}
