using CombatEffectElements;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Proxy : MonoBehaviour
{
	// change this to contain reference to actual entity with public access to combat pos via get/set
	[NonSerialized] public Vector2Int CombatPosition;

	public Slider HealthBar;
	public CombatEffectPool CombatEffectPool;
	public Image TargetIndicator;
}
