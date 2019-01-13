using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Data Container/Skills/Skill")]
public class CombatSkill : DataContainer
{
	public Sprite SkillIcon;
	public GameObject FxPrefab;
	public AudioClip castSfx;
	public float impactDelay;
	[TextArea] public string SkillDescription = "Skill Description Missing";

	[Space(20)]
	public bool CanHitEnemies = true;
	public bool CanHitAllies = false;
	public bool CanHitSelf = false;

	[Space(20)]
	public float AttackMultiplier = 1;
	public int Cooldown = 1;
	public int MinRange = 0;
    public int MaxRange = 2;
	public Vector2Int SurroundingAffectedUnits;

	[Space(20)]
	public CombatEffect[] AppliedCombatEffects;
    public CombatEffect[] SelfInflictedCombatEffects;
}