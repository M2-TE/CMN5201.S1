using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    
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

    public void OpenItemInfo(EquipmentContainer equipment, bool equipAction)
    {
        gameObject.SetActive(true);
        SingleEquipment.SetActive(true);
        SingleEquipmentIcon.sprite = equipment.ItemIcon;
        SingleEquipmentName.text = equipment.ItemName;
        SingleEquipmentDescription.text = "Description:\n" + equipment.ItemDescription;
        SingleEquipmentHP.text = equipment.HealthBonus.ToString();
        SingleEquipmentATK.text = equipment.AttackBonus.ToString();
        SingleEquipmentDEF.text = equipment.DefenseBonus.ToString();
        SingleEquipmentSPD.text = equipment.SpeedBonus.ToString();
        UpdateAction(equipAction);
    }
    
    public void OpenItemInfo(MiscContainer misc)
    {
        gameObject.SetActive(true);
        Consumable.SetActive(true);
        ConsumableIcon.sprite = misc.ItemIcon;
        ConsumableName.text = misc.ItemName;
        ConsumableEffect.text = "Effect:\n"+"THIS NEEDS TO BE DONE!";
        ConsumableDescription.text = misc.ItemDescription;
        Action.text = misc.IsUsable.Equals(Usability.ALWAYS_USABLE) || misc.IsUsable.Equals(Usability.OUT_OF_COMBAT_ONLY) ? "CONSUME" : "NONE";
    }

    public void OpenItemInfo(EquipmentContainer stored, EquipmentContainer equipped)
    {
        gameObject.SetActive(true);
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
        Action.text = "EQUIP";
    }

    public void CloseItemInfo()
    {
        CompareItems.SetActive(false);
        SingleEquipment.SetActive(false);
        Consumable.SetActive(false);
        gameObject.SetActive(false);
    }

    public void UpdateAction(bool equipAction)
    {
        if ((CompareItems.activeInHierarchy || SingleEquipment.activeInHierarchy))
        {
            if (equipAction)
            {
                Action.text = AssetManager.Instance.GetManager<CharacterInfoManager>()!= null && AssetManager.Instance.GetManager<CharacterInfoManager>().OpenCharacterPanel ? "EQUIP" : "NONE";
            }
            else
            {
                Action.text = "UNEQUIP";
            }
        }
    }
    #endregion

                                     
}
