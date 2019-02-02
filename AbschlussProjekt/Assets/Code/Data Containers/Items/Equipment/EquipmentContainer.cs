using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Data Container/Items/Equipment")]
public class EquipmentContainer : ItemContainer
{
    [Space(20)]
    [SerializeField] public Character[] MatchingClasses;
    [Space(10)]
    [SerializeField] public EquipmentSlot EquipmentType = 0;

    [Space(20)]
    [SerializeField] public int HealthBonus = 0;

    [SerializeField] public int AttackBonus = 0;

    [SerializeField] public int DefenseBonus = 0;

    [SerializeField] public int SpeedBonus = 0;

    public override ItemType GetItemType()
    {
        return ItemType.EQUIPMENT;
    }
}
