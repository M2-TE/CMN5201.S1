using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class InventoryPanel : UIPanel
{
    [SerializeField] private int inventorySize;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject inventorySlotParent;
    public ItemInfo itemInfo;

    private List<UIStorageHandler> storageSlots = new List<UIStorageHandler>();

    private InventoryManager inventoryManager;

    public int InventorySize => inventorySize;

    public bool Open => visibilityToggleNode.activeInHierarchy;

    #region Setup
    protected override void Awake()
    {
        inventoryManager = AssetManager.Instance.GetManager<InventoryManager>() ?? new InventoryManager();
        inventoryManager.InventoryPanel = this;
        base.Awake();
    }
    private void Start()
    {
        InputManager manager = AssetManager.Instance.GetManager<InputManager>();
        manager.AddListener(manager.Input.UI.InventoryOpen, ctx => ToggleVisibility());
        inventoryManager.CharacterInfoManager = AssetManager.Instance.GetManager<CharacterInfoManager>();
        InstantiateInventory();
        DisplayInventory();
    }
    private void InstantiateInventory()
    {
        for (int slot = 0; slot < inventorySize; slot++)
        {
            storageSlots.Add(Instantiate(slotPrefab, inventorySlotParent.transform).GetComponent<UIStorageHandler>());
            storageSlots[slot].Connect(this, slot);
        }
    }
    public override void ToggleVisibility(bool visibleState)
    {
        base.ToggleVisibility(visibleState);
        if (inventoryManager.CharacterInfoManager != null)
            inventoryManager.CharacterInfoManager.CharacterInfoPanel.itemInfo.UpdateAction(false);
    }
    #endregion

    public void DisplayInventory()
    {
        List<StorageSlot> Items = AssetManager.Instance.Savestate.Inventory;
        for (int position = 0; position < inventorySize; position++)
        {
            if (position < Items.Count)
                storageSlots[position].DisplayItem(Items[position]);
            else
                storageSlots[position].SetEmpty();
        }
    }

    public void DisplayItemInfo(int position, bool show)
    {
        if (!show)
        {
            itemInfo.CloseItemInfo();
            return;
        }
        ItemContainer storedItem = null;
        if (position < AssetManager.Instance.Savestate.Inventory.Count)
            storedItem = AssetManager.Instance.Savestate.Inventory[position].Item;
        if (storedItem == null)
            return;
        else if (storedItem.GetType().Equals(typeof(EquipmentContainer)))
        {
            if (AssetManager.Instance.GetManager<CharacterInfoManager>().OpenCharacterPanel)
            {
                EquipmentContainer equippedItem = AssetManager.Instance.GetManager<CharacterInfoManager>().GetItemOfCurrentCharacter(((EquipmentContainer)storedItem).EquipmentType);
                if(equippedItem != null)
                {
                    itemInfo.OpenItemInfo((EquipmentContainer)storedItem, equippedItem);
                    return;
                }
            }
            itemInfo.OpenItemInfo((EquipmentContainer)storedItem, true);
        }
        else
            itemInfo.OpenItemInfo((MiscContainer)storedItem);
    }

    public void CloseInventoryPanel()
    {
        ToggleVisibility(false);
    }

}