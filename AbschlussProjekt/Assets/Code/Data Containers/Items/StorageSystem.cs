using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemStorage", menuName = "Data Container/Storage/ItemStorage")]
public class StorageSystem : DataContainer
{
    #region Fields & Properties
    [SerializeField] public int Size = 0;
    [SerializeField] public List<StorageSlot> StorageSlots = new List<StorageSlot>();
    
    #endregion
}
