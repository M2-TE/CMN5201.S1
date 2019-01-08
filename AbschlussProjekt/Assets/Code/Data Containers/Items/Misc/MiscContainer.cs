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
    public Usability IsUsable = 0;

    [Tooltip("Optional checkbox to indicate that this item will be destroyed on use")]
    public bool DestroyOnUse;

    [Space(20)]
    public bool CanHitEnemies = false;
    public bool CanHitAllies = true;

    [Space(20)]
    public int Cooldown = 1;
    public int MinRange = 0;
    public int MaxRange = 2;
    public Vector2Int SurroundingAffectedUnits;

    [Space(20)]
    public ConsumeEffect[] ConsumeEffects;
}
