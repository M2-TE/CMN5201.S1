using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : DataContainer
{
    public string ItemName;
    public Sprite ItemIcon;
    [TextArea] public string ItemDescription = "Item Description Missing";

    [Space(20)]
    [Tooltip("The amout of money you get by selling this item")]
    public int EncumbranceValue;
    [Tooltip("The required level for using this item")]
    public int RequiredLevel;

    [Space(10)]
    [Tooltip("Optional checkbox to prevent an item from being destroyed by the player")]
    public bool IsIndestructible;
    [Tooltip("Optional checkbox to indicate that this item belongs to a quest")]
    public bool IsQuestItem;
    [Tooltip("Optional checkbox to indicate that there should only be one of these items per game")]
    public bool IsUnique;
    [Space(10)]
    [Tooltip("Optional value to limit the maximum number of items per itemslot; equals 1 if unstackable")]
    public int StackingLimit;
}
