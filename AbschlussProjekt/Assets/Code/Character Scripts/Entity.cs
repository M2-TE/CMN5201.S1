using CombatEffectElements;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Entity
{
    public string Name;
	
	#region Getters and Setters
	[NonSerialized] private AssetManager _amInstance;
	private AssetManager amInstance
	{
		get { return _amInstance ?? (_amInstance = AssetManager.Instance); }
	}

	private readonly string entityType;
    [NonSerialized] private Character charDataContainer;
    public Character CharDataContainer
    {
        get
        {
			if (charDataContainer != null) return charDataContainer;
			else return charDataContainer = amInstance.LoadBundle<Character>(amInstance.Paths.PlayableCharactersPath, entityType);
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
				return equippedRepositioningSkill = amInstance.LoadBundle<CombatSkill>(amInstance.Paths.SkillsPath, equippedRepositioningSkillString);
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
				return equippedPassSkill = amInstance.LoadBundle<CombatSkill>(amInstance.Paths.SkillsPath, equippedPassSkillString);
			else
			{
				equippedPassSkillString = CharDataContainer.PassSkill.name;
				return equippedPassSkill = CharDataContainer.PassSkill;
			}
		}
	}

    private Dictionary<EquipmentSlot, string> equippedItemStrings;
    [NonSerialized] private Dictionary<EquipmentSlot, EquipmentContainer> equippedItems;
    public EquipmentContainer GetEquippedItem(EquipmentSlot slot)
    {
        if (equippedItems == null)
            equippedItems = new Dictionary<EquipmentSlot, EquipmentContainer>();

        if (equippedItems.ContainsKey(slot) && equippedItems[slot] != null)
            return equippedItems[slot];
        else if (equippedItemStrings.ContainsKey(slot) && equippedItemStrings[slot] != "")
            return amInstance.LoadBundle<EquipmentContainer>(amInstance.Paths.EquipmentPath, equippedItemStrings[slot]);
        else
            return null;
    }
    public EquipmentContainer SetEquippedItem(EquipmentSlot slot, EquipmentContainer item)
    {
        if (equippedItems == null)
            equippedItems = new Dictionary<EquipmentSlot, EquipmentContainer>();

        EquipmentContainer previousItem = GetEquippedItem(slot);
        if (equippedItems.ContainsKey(slot))
            equippedItems[slot] = item;
        else
            equippedItems.Add(slot, item);
        if (equippedItemStrings.ContainsKey(slot))
            equippedItemStrings[slot] = item.ItemName;
        else
            equippedItemStrings.Add(slot, item.ItemName);

        return previousItem;
    }

	//private string equippedWeaponString;
 //   [NonSerialized] private EquipmentContainer equippedWeapon;
 //   public EquipmentContainer EquippedWeapon
 //   {
 //       get
 //       {
	//		if (equippedWeapon != null) return equippedWeapon;
	//		else if (equippedWeaponString != "") return equippedWeapon = amInstance.LoadBundle<EquipmentContainer>(amInstance.Paths.EquipmentPath, equippedWeaponString);
	//		else return null;
 //       }
 //       set
 //       {
 //           equippedWeapon = value;
 //           equippedWeaponString = equippedWeapon.name;
 //       }
 //   }

 //   private string equippedArmorString;
 //   [NonSerialized] private EquipmentContainer equippedArmor;
 //   public EquipmentContainer EquippedArmor
 //   {
 //       get
 //       {
 //           if (equippedArmor != null) return equippedArmor;
	//		else if (equippedArmorString != "") return equippedArmor = amInstance.LoadBundle<EquipmentContainer>(amInstance.Paths.EquipmentPath, equippedArmorString);
 //           else return null;
 //       }
 //       set
 //       {
 //           equippedArmor = value;
 //           equippedArmorString = equippedArmor.name;
 //       }
 //   }
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
        equippedItems = new Dictionary<EquipmentSlot, EquipmentContainer>();
        equippedItemStrings = new Dictionary<EquipmentSlot, string>();
    }

    public override string ToString()
    {
        return Name + ": " + baseHealth + " / " + currentHealth + " HP | "
            + currentAttack + " Atk | " + currentDefense + " Def | " + currentSpeed + " Spd";
    }

    public string Stats()
    {
        return "Stats:\n HP : " + baseHealth + "/" + currentHealth + "\n Atk  : " + currentAttack + "\n Def : " + currentDefense + "\n Spd : " + currentSpeed;
    }

    public Entity(Character charDataContainer)
    {
        Name = charDataContainer.name;

        entityType = charDataContainer.name;
        this.charDataContainer = charDataContainer;
	  
        equippedCombatSkillStrings = new string[4];
		ResetCombatSkills();

        equippedItems = new Dictionary<EquipmentSlot, EquipmentContainer>();
        equippedItemStrings = new Dictionary<EquipmentSlot, string>();

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
