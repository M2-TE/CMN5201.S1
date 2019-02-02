using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Usability
{
    UNUSABLE = 1,
    ALWAYS_USABLE = 2,
    IN_COMBAT_ONLY = 3,
    OUT_OF_COMBAT_ONLY = 4,
}

[CreateAssetMenu(fileName = "New Misc", menuName = "Data Container/Items/Misc")]
public class MiscContainer : ItemContainer
{
    
    [Space(10)]
    [SerializeField] public Usability IsUsable = 0;

    [Tooltip("Optional checkbox to indicate that this item will be destroyed on use")]
    [SerializeField] public bool DestroyOnUse;

    [Space(20)]
    [SerializeField] public bool CanHitEnemies = false;
    [SerializeField] public bool CanHitAllies = true;

    [Space(20)]
    [SerializeField] public int Cooldown = 1;
    [SerializeField] public int MinRange = 0;
    [SerializeField] public int MaxRange = 2;
    [SerializeField] public Vector2Int SurroundingAffectedUnits;

    [Space(20)]
    [SerializeField] public ConsumeEffect[] ConsumeEffects;

    public override ItemType GetItemType()
    {
        return ItemType.MISC;
    }
}
