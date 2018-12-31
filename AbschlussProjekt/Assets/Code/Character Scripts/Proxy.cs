using CombatEffectElements;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Proxy : MonoBehaviour
{
	[NonSerialized] public Vector2Int CombatPosition;

	public Slider HealthBar;
	public CombatEffectPool CombatEffectPool;
	public Image TargetIndicator;
}
