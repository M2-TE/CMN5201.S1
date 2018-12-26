using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Consumable", menuName = "Data Container/Items/Consumable")]
public class Consumable : Item
{
    public enum Usability
    {
        UNUSABLE = 1,
        ALWAYS_USABLE = 2,
        IN_COMBAT_ONLY = 3,
        OUT_OF_COMBAT_ONLY = 4,
    }
    [Space(10)]
    public Usability IsUsable = 0;

    [Tooltip("Optional checkbox to indicate that this item will be destroyed on use")]
    public bool DestroyOnUse;

}
