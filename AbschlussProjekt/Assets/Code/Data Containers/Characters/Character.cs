using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : DataContainer
{
    public GameObject Prefab;
    public Sprite Portrait;
	public float attackAnimDelay;

    [Header("Combat Stats")]
    public int BaseHealth;
    public int BaseAttack;
    public int BaseDefense;
    public int BaseSpeed;

    [Header("Combat Skills")]
    public CombatSkill[] FullSkillPool;
	public CombatSkill[] FallbackSkills;
	public CombatSkill RepositioningSkill;
	public CombatSkill PassSkill;
}
