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

    [Header("Combat Skills")]
    public CombatSkill[] FullSkillPool;
}
