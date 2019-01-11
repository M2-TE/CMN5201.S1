using CombatEffectElements;
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

	private string equippedRepositioningSkillString;
	[NonSerialized] private CombatSkill equippedRepositioningSkill;
	public CombatSkill EquippedRepositioningSkill
	{
		get
		{
			if (equippedRepositioningSkill != null) return equippedRepositioningSkill;
			else if (equippedRepositioningSkillString != null && equippedRepositioningSkillString != "")
				return equippedRepositioningSkill = AssetManager.Instance.Characters.LoadAsset<CombatSkill>(equippedRepositioningSkillString);
			else
			{
				equippedRepositioningSkillString = CharDataContainer.RepositioningSkill.name;
				return equippedRepositioningSkill = CharDataContainer.RepositioningSkill;
			}
		}
	}

	private string equippedPassSkillString;
	[NonSerialized] private CombatSkill equippedPassSkill;
	public CombatSkill EquippedPassSkill
	{
		get
		{
			if (equippedPassSkill != null) return equippedPassSkill;
			else if (equippedPassSkillString != null && equippedPassSkillString != "")
				return equippedPassSkill = AssetManager.Instance.Characters.LoadAsset<CombatSkill>(equippedPassSkillString);
			else
			{
				equippedPassSkillString = CharDataContainer.RepositioningSkill.name;
				return equippedPassSkill = CharDataContainer.RepositioningSkill;
			}
		}
	}

	private string equippedWeaponString;
    [NonSerialized] private WeaponContainer equippedWeapon;
    public WeaponContainer EquippedWeapon
    {
        get
        {
            if (equippedWeapon != null) return equippedWeapon;
            else if (equippedWeaponString != "") return equippedWeapon = AssetManager.Instance.Items.LoadAsset<WeaponContainer>(equippedWeaponString);
            else return null;
        }
        set
        {
            equippedWeapon = value;
            equippedWeaponString = equippedWeapon.name;
        }
    }

    private string equippedArmorString;
    [NonSerialized] private ArmorContainer equippedArmor;
    public ArmorContainer EquippedArmor
    {
        get
        {
            if (equippedArmor != null) return equippedArmor;
            else if (equippedArmorString != "") return equippedArmor = AssetManager.Instance.Items.LoadAsset<ArmorContainer>(equippedArmorString);
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
	public int currentMaxHealth;
    public int currentHealth;

    public int baseAttack;
    public int currentAttack;

    public int baseDefense;
    public int currentDefense;

    public int baseSpeed;
    public int currentSpeed;
    public int currentInitiative;
	
	[NonSerialized] public Vector2Int currentCombatPosition;
	[NonSerialized] public Dictionary<CombatSkill, int> currentSkillCooldowns;
	[NonSerialized] public CombatEffectPool currentCombatEffects;
    #endregion

	public void InitializeSkills()
	{
		currentSkillCooldowns = new Dictionary<CombatSkill, int>();
		for(int i = 0; i < EquippedCombatSkills.Length; i++)
		{
			if(EquippedCombatSkills[i] != null) currentSkillCooldowns.Add(EquippedCombatSkills[i], 0);
		}
	}

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
		currentMaxHealth = baseHealth;
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
