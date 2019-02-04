using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class InventoryManager : Manager
{
    [SerializeField] private Dictionary<EquipmentSlot, UIElementHandler> equippedItems = new Dictionary<EquipmentSlot, UIElementHandler>();

    private GameObject CharacterPreviewProxy;

    private int currentSelectedEntityInt = 0;

    public int CurrentSelectedEntityInt
    {
        get
        {
            return currentSelectedEntityInt;
        }
        set
        {
            currentSelectedEntityInt = value;
            currentSelectedEntity = AssetManager.Instance.Savestate.CurrentTeam[currentSelectedEntityInt];
            if(currentSelectedEntity != null)
                UpdateCharacterDisplay();
        }
    }

    private Entity currentSelectedEntity;

    public Entity CurrentSelectedEntity
    {
        get
        {
            return currentSelectedEntity ?? AssetManager.Instance.Savestate.CurrentTeam[CurrentSelectedEntityInt];
        }
        set
        {
            currentSelectedEntity = value;
        }
    }

    private InventoryPanel inventoryPanel;

    public InventoryPanel InventoryPanel
    {
        get
        {
            return inventoryPanel;
        }
        set
        {
            inventoryPanel = value;
        }
    }

    #region Items

    public void PickUpItem(int amount, ItemContainer container)
	{
		Debug.Log("Pick Up Item...");
		if(inventoryPanel.AddItemToInventory(amount, container))
        {
            Debug.Log("Item was picked up");
        }
	}

	public void EquipItem(int position)
	{
		Debug.Log("Equip Item...");
		if (inventoryPanel.EquipItem(position, out EquipmentSlot slot))
		{
			Debug.Log("Item Equiped");
            UpdateCharacterEquipmentDisplay(slot);
            inventoryPanel.ItemInfoPanel.CloseItemInfo();
        }
		else
			Debug.Log("Unable to Equip Item");
	}

	public void UnequipItem(EquipmentSlot slot)
	{
		Debug.Log("Unequip Item...");
        if (inventoryPanel.UnequipItem(slot))
        {
            Debug.Log("Item Unequiped");
            UpdateCharacterEquipmentDisplay(slot);
            inventoryPanel.ItemInfoPanel.CloseItemInfo();
        }
        else
            Debug.Log("Unable to Unequip Item");
    }

	public void ConsumeItem(int position)
	{
		Debug.Log("Consume Item...");
        if (inventoryPanel.RemoveSingleItemFromInventory(position, out ItemContainer container))
        {
            Debug.Log("Item Removed (not consumed -> Warning: there is no reference to any character stats yet... unfortunetly)");
            inventoryPanel.ItemInfoPanel.CloseItemInfo();
        }
        else
            Debug.Log("Unable to Consume Item");
    }

	public void DropItem(int position)
	{
		Debug.Log("Drop Item...");
        if (inventoryPanel.RemoveItemsFromInventory(position, out ItemContainer container, out int amount))
        {
            Debug.Log(amount + " Item(s) Removed (not dropped -> Warning: There is no parent to drop the item gameObject yet)");
            inventoryPanel.ItemInfoPanel.CloseItemInfo();
        }
        else
            Debug.Log("Unable to Drop Item");
	}

	public void DisplayItemInformation(int position, bool display)
	{
        ItemContainer item = inventoryPanel.GetItemContainerAtPosition(position);
        if (display && item != null)
            inventoryPanel.ItemInfoPanel.OpenItemInfo(item, ItemInfoType.STORAGE);
        else
            inventoryPanel.ItemInfoPanel.CloseItemInfo();
	}

    public void DisplayItemInformation(EquipmentSlot slot, bool display)
    {
        ItemContainer item = CurrentSelectedEntity.GetEquippedItem(slot);
         if (display && item != null)
            inventoryPanel.ItemInfoPanel.OpenItemInfo(item, ItemInfoType.EQUIPPED);
        else
            inventoryPanel.ItemInfoPanel.CloseItemInfo();
    }

    #endregion

    #region CharacterInfo

    public void ToggleCharacterDisplay(Entity character)
    {
        if (!inventoryPanel.IsActive)
            OpenCharacterDisplay(character);
        else
            CloseCharacterDisplay();
    }

    public void OpenCharacterDisplay(Entity character)
    {
        CurrentSelectedEntity = character;
        inventoryPanel.ToggleVisibility(true);
        inventoryPanel.CharacterInfoPanel.gameObject.SetActive(true);
        UpdateCharacterDisplay();
    }

    public void CloseCharacterDisplay()
    {
        currentSelectedEntity = null;
        inventoryPanel.ToggleVisibility(false);
    }

    public void UpdateCharacterDisplay()
    {
        if(CurrentSelectedEntity != null)
        {
            inventoryPanel.CharacterInfoPanel.DisplayCharacter(CurrentSelectedEntity);
            UpdateCharacterEquipmentDisplay();
            UpdateCharacterProxy();
        }
    }

    public void UpdateCharacterEquipmentDisplay()
    {
        foreach (EquipmentSlot slot in equippedItems.Keys)
        {
            UpdateCharacterEquipmentDisplay(slot);
        }
    }

    public void UpdateCharacterEquipmentDisplay(EquipmentSlot slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            if(CurrentSelectedEntity.GetEquippedItem(slot) != null)
            {
                equippedItems[slot].itemName = CurrentSelectedEntity.GetEquippedItem(slot).ItemName;
                equippedItems[slot].Icon.sprite = CurrentSelectedEntity.GetEquippedItem(slot).ItemIcon;
            }
            else
            {
                equippedItems[slot].SetEmpty();
            }
        }
        else
            Debug.Log("An error has occurred.");
    }

    private void UpdateCharacterProxy()
    {
        if (CharacterPreviewProxy != null)
            GameObject.Destroy(CharacterPreviewProxy);

        CharacterPreviewProxy = GameObject.Instantiate(CurrentSelectedEntity.CharDataContainer.Prefab,new Vector3(0,0,-99),Quaternion.identity,inventoryPanel.RenderCamera.transform);
        for (int child = 0; child < CharacterPreviewProxy.transform.childCount; child++)
        {
            GameObject.Destroy(CharacterPreviewProxy.transform.GetChild(child).gameObject);
        }
    }

    public bool CheckForEquippedItemAtSlot(EquipmentSlot slot, out EquipmentContainer container)
    {
        container = null;
        if (equippedItems.ContainsKey(slot))
        {
            container = (EquipmentContainer)LoadItemContainer(equippedItems[slot].itemName);
            if (container != null)
                return true;
        }
        return false;
    }

    #endregion

    public void AddHandlerToInventory(EquipmentSlot key, UIElementHandler handler)
    {
        equippedItems.Add(key, handler);
    }

    public ItemContainer LoadItemContainer(string name)
	{
        if (name == "" || name == null)
            return null;
        return AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, name);
	}
}
