using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum StoreType { SIMPLE, EQUIPED }
//Always have Unequip as the last Action Type since it can only be called on already equiped items unlike the others.
public enum EventActionType { NONE, DROP, CONSUME, EQUIP, UNEQUIP } 

public class UIElementHandler : MonoBehaviour , IPointerEnterHandler,IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler ,IScrollHandler
{
    [SerializeField] private EventActionType primaryEventType = 0;
    [SerializeField] private StoreType storeType = 0;
    [SerializeField] private EquipmentSlot equipmentSlot;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] public Image Icon;
    [SerializeField] public TextMeshProUGUI Amount;

	private InventoryManager inventoryManager;
	private int invPosition = -1;

    public string itemName;

    #region Setup

    private void Start()
	{
		inventoryManager = AssetManager.Instance.GetManager<InventoryManager>();
        if (Icon == null)
            GetComponent<Image>();
        if (storeType.Equals(StoreType.EQUIPED))
            ConnectToInventoryPanel();
	}

    public void ConnectToInventoryPanel()
    {
        AssetManager.Instance.GetManager<InventoryManager>().AddHandlerToInventory(equipmentSlot, this);
    }

    public void SetPositionInInventory(int position)
    {
        invPosition = position;
    }

    #endregion

    #region Handlerfunctions

    public void OnSelect(BaseEventData eventData)
    {
        inventoryManager.InventoryPanel.ItemInfoPanel.CurrentAction = primaryEventType;
        if (invPosition >= 0)
            inventoryManager.DisplayItemInformation(invPosition, true);
        else
            inventoryManager.DisplayItemInformation(equipmentSlot, true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        inventoryManager.DisplayItemInformation(invPosition, false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnDeselect(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPrimaryAction();
    }

    private void OnPrimaryAction()
    {
        switch (primaryEventType)
        {
            case EventActionType.DROP:
                inventoryManager.DropItem(invPosition);
                break;
            case EventActionType.CONSUME:
                inventoryManager.ConsumeItem(invPosition);
                break;
            case EventActionType.UNEQUIP:
                if (storeType.Equals(StoreType.EQUIPED))
                    inventoryManager.UnequipItem(equipmentSlot);
                break;
            case EventActionType.EQUIP:
                inventoryManager.EquipItem(invPosition);
                break;
            case EventActionType.NONE:
            default:
                Debug.Log("Magicarp used SPLASH. Nothing happens.");
                break;
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (storeType.Equals(StoreType.EQUIPED))
            return;
        if(eventData.scrollDelta.y > 0)
        {
            primaryEventType = (EventActionType)(Mathf.Max(0, ((int)primaryEventType - 1)));
        }
        else
        {
            primaryEventType = (EventActionType)(Mathf.Min(Enum.GetNames(typeof(EventActionType)).Length-2, ((int)primaryEventType + 1)));
        }
        inventoryManager.InventoryPanel.ItemInfoPanel.CurrentAction = primaryEventType;
    }
    
    #endregion

    public void SetEmpty()
    {
        Icon.sprite = emptySprite;
        itemName = "";
    }
}
