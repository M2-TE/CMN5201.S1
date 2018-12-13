using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Data Container/Skills/Skill")]
public class CombatSkill : DataContainer
{
	public Sprite SkillIcon;
	public GameObject FxPrefab;
	[TextArea] public string SkillDescription = "Skill Description Missing";
	public bool CanHitEnemies = true;
	public bool CanHitAllies = false;
	public bool CanHitSelf = false;
	public float AttackMultiplier = 1;
    public int Range = 2;
	public Vector2Int SurroundingAffectedUnits;
    public CombatEffect[] AppliedCombatEffects;
    public CombatEffect[] SelfInflictedCombatEffects;
}