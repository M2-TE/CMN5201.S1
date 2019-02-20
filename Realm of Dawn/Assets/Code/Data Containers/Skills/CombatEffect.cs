using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Effect", menuName = "Data Container/Skills/Combat Effect")]
public class CombatEffect : DataContainer
{
	public Sprite EffectSprite;
	[TextArea] public string EffectDescription = "Missing Effect Description";
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

	// Apply DoT (Damage over Time) or HoT (Heal over Time)
	public void ActivateActiveEffect(Entity affectedEntity)
	{
		//affectedEntity.currentHealth = (int)(affectedEntity.currentHealth * HealthModifier);
		//affectedEntity.currentHealth += FlatHealthModifier;
	}

	public void ApplyCombatEffectModifiers(Entity affectedEntity)
	{
		HandleEffect(affectedEntity,(x, y) => (int)(x * y - x), x => x);
	}

	public void RemoveCombatEffectModifiers(Entity affectedEntity)
	{
		HandleEffect(affectedEntity, (x, y) => (int)(x * y - x) * -1, x => - x);
	}

	private void HandleEffect(Entity affectedEntity, Func<float, float, int> floatMod, Func<int, int> intMod)
	{
		affectedEntity.CurrentMaxHealth += floatMod(affectedEntity.BaseHealth, MaxHealthModifier);
		affectedEntity.CurrentMaxHealth += intMod(FlatMaxHealthModifier);
		
		affectedEntity.CurrentAttack += floatMod(affectedEntity.BaseAttack, AttackModifier);
		affectedEntity.CurrentAttack += intMod(FlatAttackModifier);

		affectedEntity.CurrentDefense += floatMod(affectedEntity.BaseDefense, DefenseModifier);
		affectedEntity.CurrentDefense += intMod(FlatDefenseModifier);
		
		affectedEntity.CurrentSpeed += floatMod(affectedEntity.BaseSpeed, SpeedModifier);
		affectedEntity.CurrentSpeed += intMod(FlatSpeedModifier);
	}
}
