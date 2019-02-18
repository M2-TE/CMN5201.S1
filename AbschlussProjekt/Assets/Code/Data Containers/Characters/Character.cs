using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : DataContainer
{
    public GameObject Prefab;
    public Sprite Portrait;

    [Header("Combat Stats")]
    public int BaseHealth;
    public int BaseAttack;
    public int BaseDefense;
    public int BaseSpeed;

	[Header("Combat Stats")]
	public float HealthGrowth = 1f;
	public float AttackGrowth = 0f;
	public float DefenseGrowth = 0f;
	public float SpeedGrowth = 0f;

	[Header("Leveling")]
	public float baseExpYield = 5f;
	public float expYieldGrowth = 1.5f;
	public float baseExpRequirement = 10f;
	public float ExpRequirementGrowth = 1.3f;

	[Header("Combat Skills")]
    public CombatSkill[] FullSkillPool;
	public CombatSkill[] FallbackSkills;
	public CombatSkill RepositioningSkill;
	public CombatSkill PassSkill;
}
