using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPanel : UIPanel
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject itemSlotsParent;
    [SerializeField] private Chest chest;


    private List<UIStorageHandler> storageSlots = new List<UIStorageHandler>();

    private ChestManager chestManager;

    protected override void Awake()
    {
        DrawItemsFromContainer();
        chestManager = AssetManager.Instance.GetManager<ChestManager>() ?? new ChestManager();
        chestManager.ChestPanel = this;
        chestManager.Items = chest.Items;
        base.Awake();
    }

    private void Start()
    {
        chestManager.InventoryManager = AssetManager.Instance.GetManager<InventoryManager>();
        InstantiateChest();
        DisplayItemsInChest();
    }

    public void InstantiateChest()
    {
        for (int slot = 0; slot < chest.Items.Count; slot++)
        {
            storageSlots.Add(Instantiate(slotPrefab, itemSlotsParent.transform).GetComponent<UIStorageHandler>());
            storageSlots[slot].Connect(this, slot);
        }
    }

    public void DisplayItemsInChest()
    {
        for (int position = 0; position < storageSlots.Count; position++)
        {
            if (position < chestManager.Items.Count)
                storageSlots[position].DisplayItem(chestManager.Items[position]);
            else
                storageSlots[position].SetEmpty();
        }
    }

    private void DrawItemsFromContainer()
    {
        if (!chest.customChest && (chest.completeRandomness || (chest.selectedPools != null && chest.probability != null)))
            chest.SelectRandomItems();
        else if (chest.customChest)
        {
            chest.allItems = null;
            Debug.Log("Custom Chest created");
        }
        else
            Debug.LogError("A Chest has a problem choosing items from the Pools");
    }
}
