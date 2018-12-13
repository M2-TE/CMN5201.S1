using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Effect", menuName = "Data Container/Skills/Combat Effect")]
public class CombatEffect : DataContainer
{
	public bool IsActivatedAtStart;
	public Sprite EffectSprite;
	[Space(10)]
	public int Duration = 1;

	[Space(10)]
	public float MaxHealthModifier = 1;
	public int FlatMaxHealthModifier = 0;
	[Space(10)]
	public float HealthModifier = 1;
	public int FlatHealthModifier = 0;
	[Space(10)]
	public float AttackModifier = 1;
	public int FlatAttackModifier = 0;
	[Space(10)]
	public float DefenseModifier = 1;
	public int FlatDefenseModifier = 0;
	[Space(10)]
	public float SpeedModifier = 1;
	public int FlatSpeedModifier = 0;
}
