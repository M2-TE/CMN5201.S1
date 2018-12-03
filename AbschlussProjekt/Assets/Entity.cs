using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Entity
{
    public string Name;

    public string EntityType;
    [NonSerialized] public Character CharDataContainer;

    public string[] EquippedCombatSkillStrings;
    [NonSerialized] public CombatSkill[] EquippedCombatSkills;

    public string EquippedWeaponString;
    // TODO

    public string EquippedArmorString;
    // TODO


    #region Combat Stats
    public int baseHealth;
    public int currentHealth;

    public int baseAttack;
    public int currentAttack;

    public int baseDefense;
    public int currentDefense;

    public int baseSpeed;
    public int currentSpeed;
    #endregion

    public Entity Start()
    {
        CharDataContainer = AssetManager.Instance.Characters.LoadAsset<Character>(EntityType);

        EquippedCombatSkills = new CombatSkill[EquippedCombatSkillStrings.Length];
        List<CombatSkill> combatSkillPool = new List<CombatSkill>(CharDataContainer.FullSkillPool.Skills);
        for(int index = 0; index < EquippedCombatSkillStrings.Length; index++)
            EquippedCombatSkills[index] = combatSkillPool.Find(x => x.name == EquippedCombatSkillStrings[index]);

        // TODO

        return this;
    }

    public void Unload()
    {
        CharDataContainer = null;
        EquippedCombatSkills = null;
        // TODO
    }

    public Entity(Character charDataContainer)
    {
        Name = "Generic Gunwoman";
        EquippedCombatSkillStrings = new string[0];
        EquippedWeaponString = "";
        EquippedArmorString = "";

        EntityType = charDataContainer.name;

        baseHealth = charDataContainer.BaseHealth;
        currentHealth = baseHealth;

        baseAttack = charDataContainer.BaseAttack;
        currentAttack = baseAttack;

        baseDefense = charDataContainer.BaseDefense;
        currentDefense = baseDefense;

        baseSpeed = charDataContainer.BaseSpeed;
        currentSpeed = baseSpeed;
    }
}
