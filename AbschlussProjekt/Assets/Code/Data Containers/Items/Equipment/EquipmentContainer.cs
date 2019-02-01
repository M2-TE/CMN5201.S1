using UnityEngine;

public abstract class EquipmentContainer : ItemContainer
{
    [Space(20)]
    [SerializeField] public Character[] MatchingClasses;

    [SerializeField] public EquipmentSlot EquipmentType = 0;
}
