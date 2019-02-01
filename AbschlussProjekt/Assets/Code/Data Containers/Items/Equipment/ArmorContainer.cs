using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Data Container/Items/Equipment/Armor")]
public class ArmorContainer : EquipmentContainer
{
    [SerializeField] public int DefenseBonus;
}