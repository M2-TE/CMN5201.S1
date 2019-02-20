using UnityEngine;

[CreateAssetMenu(fileName = "Combat Manager Settings", menuName = "Data Container/Settings/Combat Manager Settings")]
public class CombatManagerSettings : Settings
{
	public LayerMask clickableLayers;
	public Color lowerStatColor;
	public Color higherStatColor;
	public float healthbarAdjustmentSpeed = 10f;
}