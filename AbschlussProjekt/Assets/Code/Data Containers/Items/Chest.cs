using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest", menuName = "Data Container/Storage/Chest")]
public class Chest : StorageSystem
{
    [SerializeField] public ItemContainer[] items;
}
