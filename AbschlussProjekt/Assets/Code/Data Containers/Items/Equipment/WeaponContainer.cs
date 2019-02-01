using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Data Container/Items/Equipment/Weapon")]
public class WeaponContainer : EquipmentContainer
{
    [SerializeField] public int AttackBonus;
}
