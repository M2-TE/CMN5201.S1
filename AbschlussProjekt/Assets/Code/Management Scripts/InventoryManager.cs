using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Inventory inventory;

    private bool InventoryOpen { get{ return inventoryPanel.activeSelf; } }

    // Make sure the Inventory is Closed at the start of a Scene
    private void OnEnable()
    {
        {
            inventoryPanel.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        CheckUserKeyInput();
	}

    private void CheckUserKeyInput()
    {
        if(Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryPanel.gameObject.SetActive(!InventoryOpen);
        }
    }

    public void PickUpItem(int amount,ItemContainer container)
    {
        Debug.Log("Pick Up Item");
        inventory.AddItemToInventory(amount, container);
    }

    public void EquipItem(int position)
    {
        Debug.Log("Equip Item");
        if (inventory.EquipItem(position))
        {
            Debug.Log("Equiping was a success");
        }
        else
            Debug.Log("FAIL");
    }

    public void UnEquipItem(EquipmentSlot equipmentSlot)
    {
        Debug.Log("UnEquip Item");
    }

    public void ConsumeItem(int position)
    {
        Debug.Log("Consume Item");
    }

    public void DropItem(int position)
    {
        Debug.Log("Drop Item");
    }

    public void DisplayInformation(bool display)
    {
        Debug.Log("Display Information");
    }

    public ItemContainer LoadItemContainer(string name)
    {
        return AssetManager.Instance.Items.LoadAsset<ItemContainer>(name);
    }
}
