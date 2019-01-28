using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private Inventory inventory;

    private LayerMask clickableLayers;
    private bool InventoryOpen { get{ return inventoryPanel.gameObject.activeSelf; } }

    private void Start()
    {
        clickableLayers = AssetManager.Instance.Settings.LoadAsset<MiscSettings>("Misc Settings").clickableLayers;
    }

    private void Update()
    {
        CheckUserKeyInput();
        if (InventoryOpen)
        {
            CheckUserHovering();
        }

	}

    private void CheckUserHovering()
    {
        RaycastHit2D hit = Physics2D.Raycast(AssetManager.Instance.MainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, clickableLayers);
        if (hit.collider != null && hit.collider.GetComponent<HitBehaviour>() != null)
        {
            if (!CheckUserMouseInput(hit))
            {
                hit.collider.GetComponent<HitBehaviour>().UserInputHit(HitType.HOVER, this);
            }
        }
    }

    private bool CheckUserMouseInput(RaycastHit2D hit)
    {
        if (Input.GetMouseButtonUp(0))
        {
            hit.collider.GetComponent<HitBehaviour>().UserInputHit(HitType.LEFTCLICK, this);
            return true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            hit.collider.GetComponent<HitBehaviour>().UserInputHit(HitType.RIGHTCLICK, this);
            return true;
        }
        return false;
    }

    private void CheckUserKeyInput()
    {
        if(Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryPanel.gameObject.SetActive(!InventoryOpen);
        }
    }

    public void EquipItem(StorageSlot inventoryItem)
    {

    }

    public void UnEquipItem(EquipmentSlot equipmentSlot)
    {

    }

    public void ConsumeItem(StorageSlot inventoryItem)
    {

    }

    public void DisplayOptions(HitActionType type)
    {

    }

    public void DisplayInformation()
    {

    }
}
