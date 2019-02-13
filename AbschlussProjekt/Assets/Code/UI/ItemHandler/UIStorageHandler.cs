using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStorageHandler : UIElementHandler
{
    [SerializeField] private TextMeshProUGUI amount;

    private InventoryPanel inventoryPanel;
    private ChestPanel chestPanel;
    private int position;

    public TextMeshProUGUI Amount => amount;

    public void Connect(InventoryPanel panel, int inventoryPosition)
    {
        inventoryPanel = panel;
        position = inventoryPosition;
    }

    public void Connect(ChestPanel panel, int chestPosition)
    {
        chestPanel = panel;
        position = chestPosition;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if(inventoryPanel != null)
            inventoryPanel.DisplayItemInfo(position, true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if(inventoryPanel != null)
            inventoryPanel.DisplayItemInfo(position, false);
    }

    protected override void OnPrimaryAction()
    {
        if (inventoryPanel != null)
            AssetManager.Instance.GetManager<InventoryManager>().PrimaryActionOnItem(position);
        else if (chestPanel != null)
            AssetManager.Instance.GetManager<ChestManager>().PrimaryActionOnItem(position);
        else
            Debug.LogError("An Error has occurred");
    }

    public void DisplayItem(StorageSlot item)
    {
        Activate(true);
        Icon.sprite = AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, item.content).ItemIcon;
        Amount.text = item.amount.ToString();
    }

    public override void SetEmpty()
    {
        Activate(false);
    }

    private void Activate(bool enabled)
    {
        empty = !enabled;
        Icon.enabled = enabled;
        Amount.enabled = enabled;
    }
}
