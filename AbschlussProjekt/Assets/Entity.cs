using System;
using System.Collections.Generic;

[Serializable]
public class Entity
{
    public string Name;

    public string EntityType;
    [NonSerialized] public Character CharDataContainer;

    public string[] EquippedCombatSkillStrings;
    [NonSerialized] public CombatSkill[] EquippedCombatSkills;

    public string EquippedWeaponString;
    [NonSerialized] public Weapon EquippedWeapon;

    public string EquippedArmorString;
    [NonSerialized] public Armor EquippedArmor;


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

    public Entity Init()
    {
        CharDataContainer = AssetManager.Instance.Characters.LoadAsset<Character>(EntityType);

        EquippedCombatSkills = new CombatSkill[EquippedCombatSkillStrings.Length];
        List<CombatSkill> combatSkillPool = new List<CombatSkill>(CharDataContainer.FullSkillPool);
        for(int index = 0; index < EquippedCombatSkillStrings.Length; index++)
            EquippedCombatSkills[index] = combatSkillPool.Find(x => x.name == EquippedCombatSkillStrings[index]);

        if (EquippedWeaponString != "") EquippedWeapon = AssetManager.Instance.Equipment.LoadAsset<Weapon>(EquippedWeaponString);
        if (EquippedArmorString != "") EquippedArmor = AssetManager.Instance.Equipment.LoadAsset<Armor>(EquippedArmorString);

        return this;
    }

    public void Unload()
    {
        CharDataContainer = null;
        EquippedCombatSkills = null;
        EquippedWeapon = null;
        EquippedArmor = null;
    }

    public Entity(Character charDataContainer)
    {
        Name = "Generic Gunwoman";

        EntityType = charDataContainer.name;
        CharDataContainer = charDataContainer;

        EquippedCombatSkillStrings = new string[0];

        EquippedWeaponString = "";

        EquippedArmorString = "";


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
