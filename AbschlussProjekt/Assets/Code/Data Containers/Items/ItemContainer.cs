using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { EQUIPMENT, MISC, NEITHER}

public abstract class ItemContainer : DataContainer
{
    [SerializeField] public string ItemName;
    [SerializeField] public Sprite ItemIcon;
    [TextArea][SerializeField] public string ItemDescription = "Item Description Missing";

    [Space(20)]
    [Tooltip("The amout of money you get by selling this item")]
    [SerializeField] public int EncumbranceValue;
    [Tooltip("The required level for using this item")]
    [SerializeField] public int LevelRequirement;

    [Space(10)]
    [Tooltip("Optional checkbox to prevent an item from being destroyed by the player")]
    [SerializeField] public bool IsIndestructible;
    [Tooltip("Optional checkbox to indicate that this item belongs to a quest")]
    [SerializeField] public bool IsQuestItem;
    [Tooltip("Optional checkbox to indicate that there should only be one of these items per game")]
    [SerializeField] public bool IsUnique;
    [Space(10)]
    [Tooltip("Optional value to limit the maximum number of items per itemslot; equals 1 if unstackable")]
    [SerializeField] public int StackingLimit;

    public void OnEnable()
    {
        ItemName = name;
    }

    public abstract ItemType GetItemType();
}
