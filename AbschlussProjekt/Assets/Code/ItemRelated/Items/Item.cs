using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    #region Fields
    public int ID;

    public string Name;

    public Sprite ItemIcon;
    public string ItemDescription;

    public int EncumbranceValue;
    public int LevelRequirement;

    public bool IsIndestructible;
    public bool IsQuestItem;
    public bool IsUnique;

    public int StackingLimit;

    public int currentlyStacked;

    public int CurrentlyStacked {  get { return currentlyStacked; } set { currentlyStacked = (value > StackingLimit) || (value < 1) ? currentlyStacked : value; } }
    #endregion

    public Item(ItemContainer itemDataContainer)
    {
        ID = itemDataContainer.GetInstanceID();

        Name = itemDataContainer.ItemName;

        Debug.Log("Item name: " + Name + "\nItem ID: "+ID);

        ItemIcon = itemDataContainer.ItemIcon;
        ItemDescription = itemDataContainer.ItemDescription;

        EncumbranceValue = itemDataContainer.EncumbranceValue;
        LevelRequirement = itemDataContainer.LevelRequirement;

        IsIndestructible = itemDataContainer.IsIndestructible;
        IsQuestItem = itemDataContainer.IsQuestItem;
        IsUnique = itemDataContainer.IsUnique;

        StackingLimit = itemDataContainer.StackingLimit;
    }
}
