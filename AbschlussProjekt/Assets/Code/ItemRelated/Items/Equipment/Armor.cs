using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment
{
    public int defenseBonus;

    public Armor(ArmorContainer armorDataContainer) : base(armorDataContainer)
    {
        defenseBonus = armorDataContainer.defenseBonus;
    }
}
