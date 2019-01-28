using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType { HOVER,  LEFTCLICK, RIGHTCLICK }
public enum HitActionType { MISC, UNEQUIPED, EQUIPED }

public class HitBehaviour : MonoBehaviour
{
    [SerializeField] private HitActionType hitActionType = 0;
    private StorageSlot Item
    {
        get
        {
            return GetComponent<StorageSlot>();
        }
    }
    [SerializeField] private EquipmentSlot slot;

    public void UserInputHit(HitType type, InventoryManager manager)
    {

        switch (type)
        {
            case HitType.HOVER:
                manager.DisplayInformation();
                break;
            case HitType.RIGHTCLICK:
                manager.DisplayOptions(hitActionType);
                break;
            case HitType.LEFTCLICK:
                UserInput(manager);
                break;
            default:
                break;
        }
    }

    private void UserInput(InventoryManager manager)
    {
        switch (hitActionType)
        {
            case HitActionType.MISC:
                manager.ConsumeItem(Item);
                break;
            case HitActionType.UNEQUIPED:
                manager.UnEquipItem(slot);
                break;
            case HitActionType.EQUIPED:
                manager.EquipItem(Item);
                break;
            default:
                break;
        }
    }
}
