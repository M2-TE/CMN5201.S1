using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Misc Settings", menuName = "Asset Containers/Misc Settings", order = 1)]
public class MiscSettings : ScriptableObject
{
    public LayerMask GroundLayers;
}