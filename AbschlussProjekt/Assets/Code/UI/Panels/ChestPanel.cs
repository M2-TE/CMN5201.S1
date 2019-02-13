using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPanel : UIPanel
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject itemSlotsParent;

    public Chest Chest;

    private List<UIStorageHandler> storageSlots = new List<UIStorageHandler>();

    private ChestManager chestManager;

    protected override void Awake()
    {
        chestManager = AssetManager.Instance.GetManager<ChestManager>() ?? new ChestManager();
        chestManager.ChestPanel = this;
        if (!Chest.customChest && (Chest.completeRandomness || (Chest.selectedPools != null && Chest.probability != null)))
            Chest.SelectRandomItems();
        else if (Chest.customChest)
        {
            Chest.allItems = null;
            Debug.Log("Custom Chest created");
        }
        else
            Debug.LogError("A Chest has a problem choosing items from the Pools");
        base.Awake();
    }

    private void Start()
    {
        chestManager.InventoryManager = AssetManager.Instance.GetManager<InventoryManager>();
        DisplayChestItems(true);
    }

    public void DisplayChestItems(bool firstTime)
    {
        if (!firstTime)
        {
            for (int i = 0; i < storageSlots.Count; i++)
            {
                Destroy(storageSlots[i].gameObject);
            }
        }
        for (int slot = 0; slot < Chest.Items.Count; slot++)
        {
            storageSlots.Add(Instantiate(slotPrefab, itemSlotsParent.transform).GetComponent<UIStorageHandler>());
            storageSlots[slot].Connect(this, slot);
            storageSlots[slot].DisplayItem(Chest.Items[slot]);
        }
    }

    public void OpenChest()
    {
        gameObject.SetActive(true);
        if (AssetManager.Instance.GetManager<InventoryManager>() != null)
            AssetManager.Instance.GetManager<InventoryManager>().InventoryPanel.ToggleVisibility(false);
    }
}
