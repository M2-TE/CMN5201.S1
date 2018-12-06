using UnityEngine;

[CreateAssetMenu(fileName = "Misc Settings", menuName = "Data Container/Misc Settings")]
public class MiscSettings : Settings
{
    public LayerMask GroundLayers;
	public LayerMask clickableLayers;
}