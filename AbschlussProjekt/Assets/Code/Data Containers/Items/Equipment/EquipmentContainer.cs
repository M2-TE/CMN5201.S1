using UnityEngine;

public abstract class EquipmentContainer : ItemContainer
{
    [Space(20)]
    [SerializeField] public Character[] MatchingClasses;

    [SerializeField] public EquipmentSlots EquipmentType = 0;
}
