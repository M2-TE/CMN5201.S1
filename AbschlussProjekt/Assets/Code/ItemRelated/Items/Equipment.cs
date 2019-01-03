using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    #region Fields
    public Character[] MatchingClasses;
    #endregion


    public Equipment(EquipmentContainer equipmentDataContainer) : base(equipmentDataContainer)
    {
        MatchingClasses = equipmentDataContainer.MatchingClasses;
    }
}
