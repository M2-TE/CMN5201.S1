using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Pool", menuName = "Data Container/Skills/Skill")]
public class CombatSkill : DataContainer
{
    public Sprite SkillIcon;
    public float DamageMultiplier;
    public int Range;
    public bool[] AppliedEffects;
    public bool[] SelfInflictedEffects;
}