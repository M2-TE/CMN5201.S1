using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Effect", menuName = "Data Container/Skills/Combat Effect")]
public class CombatEffect : DataContainer
{
	public bool IsBuff;
	public bool IsActivatedAtStart;
	[Space(10)]
	public int FlatMaxHealthModifier;
	public float MaxHealthModifier;
	[Space(10)]
	public int FlatHealthModifier;
	public float HealthModifier;
	[Space(10)]
	public int FlatAttackModifier;
	public float AttackModifier;
	[Space(10)]
	public int FlatDefenseModifier;
	public float DefenseModifier;
	[Space(10)]
	public int FlatSpeedModifier;
	public float SpeedModifier;
}
