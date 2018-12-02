using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Pool", menuName = "Data Container/Skills/Skill Pool")]
public class CombatSkillPool : DataContainer
{
    public CombatSkill[] Skills;
}
