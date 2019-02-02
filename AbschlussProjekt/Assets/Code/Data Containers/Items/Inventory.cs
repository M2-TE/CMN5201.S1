using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Data Container/Storage/Inventory")]
public class Inventory : StorageSystem
{
    public int Currency;
    [HideInInspector] public int CurrentSelectedEntityInt;

    public Entity CurrentSelectedEntity
    {
        get
        {
            return AssetManager.Instance.Savestate.CurrentTeam[CurrentSelectedEntityInt];
        }
    }
}
