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

public class UIElementHandler : MonoBehaviour , IPointerEnterHandler,IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, IScrollHandler
{
    [SerializeField] private EventActionType primaryEventType = 0;
    [SerializeField] private StoreType storeType = 0;
    [SerializeField] private EquipmentSlot equipmentSlot;
    [SerializeField] public Image Icon;
    [SerializeField] public TextMeshProUGUI Amount;

	private InventoryManager manager;
	private int invPosition = -1;

	private void Start()
	{
		manager = AssetManager.Instance.GetManager<InventoryManager>();
	}

	public void OnSelect(BaseEventData eventData)
    {
        manager.DisplayInformation(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        manager.DisplayInformation(false);
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
                manager.DropItem(invPosition);
                break;
            case EventActionType.CONSUME:
                manager.ConsumeItem(invPosition);
                break;
            case EventActionType.UNEQUIP:
                if (storeType.Equals(StoreType.EQUIPED))
                    manager.UnEquipItem(equipmentSlot);
                break;
            case EventActionType.EQUIP:
                manager.EquipItem(invPosition);
                break;
            case EventActionType.NONE:
            default:
                Debug.Log("Karpador used SPLASH. Nothing happens.");
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
    }

    public void SetPositionInInventory(int position)
    {
        invPosition = position;
    }
}
