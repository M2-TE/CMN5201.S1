using UnityEngine;

[CreateAssetMenu(fileName = "Misc Settings", menuName = "Data Container/Settings/Misc Settings")]
public class MiscSettings : Settings
{
    public LayerMask GroundLayers;
	public AnimationCurve WobbleCurve;
	public float loadingScreenFadeDuration;
}