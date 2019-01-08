using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misc : Item
{
    #region Fields
    public Usability IsUsable;

    public bool DestroyOnUse;

    public bool CanHitEnemies;
    public bool CanHitAllies;

    public int Cooldown;
    public int MinRange;
    public int MaxRange;
    public Vector2Int SurroundingAffectedUnits;

    public ConsumeEffect[] ConsumeEffects;
    #endregion

    public Misc(MiscContainer miscDataContainer) : base(miscDataContainer)
    {
        IsUsable = miscDataContainer.IsUsable;

        DestroyOnUse = miscDataContainer.DestroyOnUse;

        CanHitEnemies = miscDataContainer.CanHitEnemies;
        CanHitAllies = miscDataContainer.CanHitAllies;

        Cooldown = miscDataContainer.Cooldown;
        MinRange = miscDataContainer.MinRange;
        MaxRange = miscDataContainer.MaxRange;
        SurroundingAffectedUnits = miscDataContainer.SurroundingAffectedUnits;

        ConsumeEffects = miscDataContainer.ConsumeEffects;
    }
}
