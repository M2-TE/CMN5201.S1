using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoManager : Manager
{
    public CharacterInfoPanel CharacterInfoPanel;

    public InventoryManager InventoryManager;

    public bool OpenCharacterPanel => CharacterInfoPanel.Open;

    public EquipmentContainer GetItemOfCurrentCharacter(EquipmentSlot slot)
    {
        return CharacterInfoPanel.CurrentCharacter.GetEquippedItem(slot);
    }

    public EquipmentContainer RemoveItemOfCurrentCharacter(EquipmentSlot slot)
    {
        HandleStatsEffect(CharacterInfoPanel.CurrentCharacter, GetItemOfCurrentCharacter(slot), x => - x);
        return CharacterInfoPanel.CurrentCharacter.SetEquippedItem(slot, null);
    }

    public void SetItemOfCurrentCharacter(EquipmentContainer item)
    {
        HandleStatsEffect(CharacterInfoPanel.CurrentCharacter, item, x => x);
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
            InventoryManager.TakeItem(1, RemoveItemOfCurrentCharacter(slot));
        else
            AssetManager.Instance.Savestate.Inventory.Add(new StorageSlot(1, RemoveItemOfCurrentCharacter(slot).ItemName));
        CharacterInfoPanel.DisplayEquipmentSlot(slot);
    }

    public void EquipItem(EquipmentContainer item)
    {
        PrimaryActionOnItem(item.EquipmentType);
        SetItemOfCurrentCharacter(item);
        CharacterInfoPanel.DisplayEquipmentSlot(item.EquipmentType);
    }

    private void HandleStatsEffect(Entity affectedEntity,EquipmentContainer item, Func<int,int> modification)
    {
        affectedEntity.CurrentHealth += modification(item.HealthBonus);

        affectedEntity.CurrentAttack += modification(item.AttackBonus);

        affectedEntity.CurrentDefense += modification(item.DefenseBonus);

        affectedEntity.CurrentSpeed += modification(item.SpeedBonus);

        CharacterInfoPanel.DisplayCharacterStats();
    }
}
