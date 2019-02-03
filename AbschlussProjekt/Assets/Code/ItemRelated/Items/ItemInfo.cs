using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemInfoType { STORAGE, EQUIPPED }

public class ItemInfo : MonoBehaviour
{
    private InventoryManager manager;

    private EventActionType  currentAction = 0;

    public EventActionType CurrentAction
    {
        get => currentAction;
        set
        {
            currentAction = value;
            UpdateActionPanel();
        }
    }

    public void SetInventoryManager(InventoryManager manager)
    {
        this.manager = manager;
    }
    
    #region UIReferences

    [SerializeField] private GameObject CompareItems, SingleEquipment, Consumable;
    [SerializeField] private Image CompareItemsLeftIcon, CompareItemsRightIcon,
                                   SingleEquipmentIcon,
                                   ConsumableIcon;

    [SerializeField] private TextMeshProUGUI CompareItemsLeftHP, CompareItemsLeftATK, CompareItemsLeftDEF, CompareItemsLeftSPD,
                                             CompareItemsRightHP, CompareItemsRightATK, CompareItemsRightDEF, CompareItemsRightSPD,
                                             SingleEquipmentName, SingleEquipmentHP, SingleEquipmentATK, SingleEquipmentDEF, SingleEquipmentSPD, SingleEquipmentDescription,
                                             ConsumableName, ConsumableEffect, ConsumableDescription,
                                             Action;

    #endregion

    #region Open / Close Panels

    public void OpenItemInfo(ItemContainer item, ItemInfoType type)
    {
        gameObject.SetActive(true);
        UpdateActionPanel();
        if (type.Equals(ItemInfoType.STORAGE))
        {
            if (item.GetType().Equals(typeof(MiscContainer)))
            {
                OpenCosumableInfo((MiscContainer)item);
                return;
            }
            else if (currentAction.Equals(EventActionType.EQUIP) && manager.CheckForEquippedItemAtSlot(((EquipmentContainer)item).EquipmentType, out EquipmentContainer equipped))
            {
                OpenCompareEquipmentInfo((EquipmentContainer)item, equipped);
                return;
            }
        }

        OpenSingleEquipmentInfo((EquipmentContainer)item);
    }

    private void OpenSingleEquipmentInfo(EquipmentContainer equipment)
    {
        SingleEquipment.SetActive(true);
        SingleEquipmentIcon.sprite = equipment.ItemIcon;
        SingleEquipmentName.text = equipment.ItemName;
        SingleEquipmentDescription.text = "Description:\n" + equipment.ItemDescription;
        SingleEquipmentHP.text = equipment.HealthBonus.ToString();
        SingleEquipmentATK.text = equipment.AttackBonus.ToString();
        SingleEquipmentDEF.text = equipment.DefenseBonus.ToString();
        SingleEquipmentSPD.text = equipment.SpeedBonus.ToString();
    }
    
    private void OpenCosumableInfo(MiscContainer consumable)
    {
        Consumable.SetActive(true);
        ConsumableIcon.sprite = consumable.ItemIcon;
        ConsumableName.text = consumable.ItemName;
        ConsumableEffect.text = "Effect:\n"+"THIS NEEDS TO BE DONE!";
        ConsumableDescription.text = consumable.ItemDescription;
    }

    private void OpenCompareEquipmentInfo(EquipmentContainer stored, EquipmentContainer equipped)
    {
        CompareItems.SetActive(true);
        CompareItemsLeftIcon.sprite = stored.ItemIcon;
        CompareItemsLeftHP.text = stored.HealthBonus.ToString();
        CompareItemsLeftATK.text = stored.AttackBonus.ToString();
        CompareItemsLeftDEF.text = stored.DefenseBonus.ToString();
        CompareItemsLeftSPD.text = stored.SpeedBonus.ToString();

        CompareItemsRightIcon.sprite = equipped.ItemIcon;
        CompareItemsRightHP.text = equipped.HealthBonus.ToString();
        CompareItemsRightATK.text = equipped.AttackBonus.ToString();
        CompareItemsRightDEF.text = equipped.DefenseBonus.ToString();
        CompareItemsRightSPD.text = equipped.SpeedBonus.ToString();
    }

    private void UpdateActionPanel()
    {
        switch (currentAction)
        {
            case EventActionType.NONE:
                Action.text = "NONE";
                break;
            case EventActionType.DROP:
                Action.text = "DROP";
                break;
            case EventActionType.CONSUME:
                Action.text = "CONSUME";
                break;
            case EventActionType.EQUIP:
                Action.text = "EQUIP";
                break;
            case EventActionType.UNEQUIP:
                Action.text = "UNEQUIP";
                break;
            default:
                break;
        }
    }

    public void CloseItemInfo()
    {
        CompareItems.SetActive(false);
        SingleEquipment.SetActive(false);
        Consumable.SetActive(false);
        gameObject.SetActive(false);
    }

    #endregion

                                     
}
