using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Data Container/Skills/Skill")]
public class CombatSkill : DataContainer
{
    public Sprite SkillIcon;
	public GameObject FxPrefab;
    public float DamageMultiplier;
    public int Range;
    public CombatEffect[] AppliedCombatEffects;
    public CombatEffect[] SelfInflictedCombatEffects;
}