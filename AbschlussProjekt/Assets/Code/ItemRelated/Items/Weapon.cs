using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment {

    public int attackBonus;

    public Weapon(WeaponContainer weaponDataContainer) : base(weaponDataContainer)
    {
        attackBonus = weaponDataContainer.attackBonus;
    }
}
