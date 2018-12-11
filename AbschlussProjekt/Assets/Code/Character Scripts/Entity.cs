using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Entity
{
    public string Name;

    #region Getters and Setters
    private readonly string entityType;
    [NonSerialized] private Character charDataContainer;
    public Character CharDataContainer
    {
        get
        {
            if (charDataContainer != null) return charDataContainer;
            else return charDataContainer = AssetManager.Instance.Characters.LoadAsset<Character>(entityType);
        }
    }

	private string[] equippedCombatSkillStrings;
    [NonSerialized] private CombatSkill[] equippedCombatSkills;
    public CombatSkill[] EquippedCombatSkills
    {
        get
        {
            if (equippedCombatSkills != null) return equippedCombatSkills;
            else return LoadCombatSkills();
		}
    }

	private string equippedWeaponString;
    [NonSerialized] private Weapon equippedWeapon;
    public Weapon EquippedWeapon
    {
        get
        {
            if (equippedWeapon != null) return equippedWeapon;
            else if (equippedWeaponString != "") return equippedWeapon = AssetManager.Instance.Equipment.LoadAsset<Weapon>(equippedWeaponString);
            else return null;
        }
        set
        {
            equippedWeapon = value;
            equippedWeaponString = equippedWeapon.name;
        }
    }

    private string equippedArmorString;
    [NonSerialized] private Armor equippedArmor;
    public Armor EquippedArmor
    {
        get
        {
            if (equippedArmor != null) return equippedArmor;
            else if (equippedArmorString != "") return equippedArmor = AssetManager.Instance.Equipment.LoadAsset<Armor>(equippedArmorString);
            else return null;
        }
        set
        {
            equippedArmor = value;
            equippedArmorString = equippedArmor.name;
        }
    }
	#endregion

	#region Combat Skills
	private CombatSkill[] LoadCombatSkills()
	{
		List<CombatSkill> combatSkillPool = new List<CombatSkill>(CharDataContainer.FullSkillPool);
		equippedCombatSkills = new CombatSkill[equippedCombatSkillStrings.Length];
		for (int i = 0; i < equippedCombatSkillStrings.Length; i++)
			equippedCombatSkills[i] = combatSkillPool.Find(x => x.name == equippedCombatSkillStrings[i]);

		if (equippedCombatSkills[0] == null) ResetCombatSkills();

		return equippedCombatSkills;
	}
	public CombatSkill SetCombatSkill(int index, CombatSkill combatSkill)
	{
		equippedCombatSkillStrings[index] = combatSkill.name;
		return equippedCombatSkills[index] = combatSkill;
	}
	public CombatSkill[] SetCombatSkills(CombatSkill[] skills)
	{
		if (skills.Length != 4)
		{
			equippedCombatSkills = new CombatSkill[4];
			for (int i = 0; i < equippedCombatSkills.Length; i++)
				equippedCombatSkills[i] = (skills.Length > i) ? skills[i] : null;
		}
		else  equippedCombatSkills = skills;

		for (int i = 0; i < equippedCombatSkills.Length; i++)
			equippedCombatSkillStrings[i] = (equippedCombatSkills[i] == null) ? null : equippedCombatSkills[i].name;
		return equippedCombatSkills;
	}
	public void ResetCombatSkills()
	{
		SetCombatSkills(CharDataContainer.FallbackSkills);
	}
	#endregion

	#region Combat Stats
	public int baseHealth;
    public int currentHealth;

    public int baseAttack;
    public int currentAttack;

    public int baseDefense;
    public int currentDefense;

    public int baseSpeed;
    public int currentSpeed;
    public int currentInitiative;
	
	[NonSerialized] public Vector2Int currentCombatPosition;
    #endregion

    public void Unload()
    {
        charDataContainer = null;
        equippedCombatSkills = null;
        equippedWeapon = null;
        equippedArmor = null;
    }

    public override string ToString()
    {
        return Name + ": " + baseHealth + " / " + currentHealth + " HP | "
            + currentAttack + " Atk | " + currentDefense + " Def | " + currentSpeed + " Spd";
    }

    public Entity(Character charDataContainer)
    {
        Name = charDataContainer.name;

        entityType = charDataContainer.name;
        this.charDataContainer = charDataContainer;


	  
        equippedCombatSkillStrings = new string[4];
		ResetCombatSkills();

        equippedWeaponString = "";

        equippedArmorString = "";




        baseHealth = charDataContainer.BaseHealth;
        currentHealth = baseHealth;

        baseAttack = charDataContainer.BaseAttack;
        currentAttack = baseAttack;

        baseDefense = charDataContainer.BaseDefense;
        currentDefense = baseDefense;

        baseSpeed = charDataContainer.BaseSpeed;
        currentSpeed = baseSpeed;
        currentInitiative = 0;
    }
}
