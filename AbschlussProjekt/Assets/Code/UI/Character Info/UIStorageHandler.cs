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
    private int inventoryPosition;

    public TextMeshProUGUI Amount => amount;

    public void Connect(InventoryPanel panel, int inventoryPosition)
    {
        inventoryPanel = panel;
        this.inventoryPosition = inventoryPosition;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        inventoryPanel.DisplayItemInfo(inventoryPosition, true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        inventoryPanel.DisplayItemInfo(inventoryPosition, false);
    }

    protected override void OnPrimaryAction()
    {
        AssetManager.Instance.GetManager<InventoryManager>().PrimaryActionOnItem(inventoryPosition);
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
