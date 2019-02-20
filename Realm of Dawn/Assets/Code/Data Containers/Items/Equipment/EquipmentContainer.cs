using UnityEngine;

public enum PlayableChars { GUNWOMAN, KNIGHT, MAGE, PRIEST, ROBOT, SKELETON, SKELETON_ARCHER, SKELETON_CHIEF, WOLF}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Data Container/Items/Equipment")]
public class EquipmentContainer : ItemContainer
{

    [Space(20)]
    [SerializeField] public PlayableChars[] MatchingClasses;
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

    public bool CheckMatchingClass(PlayableChars m_class)
    {
        Debug.Log(m_class.ToString());
        if (MatchingClasses == null || MatchingClasses.Length == 0)
            return true;
        for (int i = 0; i < MatchingClasses.Length; i++)
        {
            if (MatchingClasses[i].Equals(m_class))
                return true;
        }
        return false;
    }
}
