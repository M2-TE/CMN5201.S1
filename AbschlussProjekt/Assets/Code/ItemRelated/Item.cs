using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    #region Variables
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
    #endregion

    #region Stacking functions
    public bool TryAddItemAmount(int amount)
    {
        if (amount + currentlyStacked > StackingLimit)
            return false;
        currentlyStacked += amount;
        return true;
    }
    public bool TryRemoveItemAmount(int amount)
    {
        if (currentlyStacked - amount < 0)
            return false;
        currentlyStacked -= amount;
        return true;
    }
    #endregion

    // The information stored in the stats variables will be gained through Asset bundles! Coming soon.
}
