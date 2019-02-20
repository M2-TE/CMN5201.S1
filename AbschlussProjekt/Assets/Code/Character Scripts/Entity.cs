using CombatEffectElements;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class Entity
{
    public string Name;

    public PlayableChars CharacterType;

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
                return equippedRepositioningSkill = amInstance.LoadBundle<CombatSkill>(amInstance.Paths.PlayableCharactersPath, equippedRepositioningSkillString);
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
                return equippedPassSkill = amInstance.LoadBundle<CombatSkill>(amInstance.Paths.PlayableCharactersPath, equippedPassSkillString);
            else
            {
                equippedPassSkillString = CharDataContainer.PassSkill.name;
                return equippedPassSkill = CharDataContainer.PassSkill;
            }
        }
    }

    private int[] skillLevelRequirements;
    public int[] SkillLevelRequirements => skillLevelRequirements ?? (skillLevelRequirements = new int[3]{1,3,5});

    private Dictionary<EquipmentSlot, string> equippedItemStrings;
    [NonSerialized] private Dictionary<EquipmentSlot, EquipmentContainer> equippedItems;
    public EquipmentContainer GetEquippedItem(EquipmentSlot slot)
    {
        if (equippedItems == null)
            equippedItems = new Dictionary<EquipmentSlot, EquipmentContainer>();
        if (equippedItemStrings == null)
            equippedItemStrings = new Dictionary<EquipmentSlot, string>();

        if (equippedItems.ContainsKey(slot) && equippedItems[slot] != null)
            return equippedItems[slot];
        else if (equippedItemStrings.ContainsKey(slot) && equippedItemStrings[slot] != "" && equippedItemStrings[slot] != null)
            return amInstance.LoadBundle<EquipmentContainer>(amInstance.Paths.ItemsPath, equippedItemStrings[slot]);
        else
            return null;
    }
    public EquipmentContainer SetEquippedItem(EquipmentSlot slot, EquipmentContainer item)
    {
        if (equippedItems == null)
            equippedItems = new Dictionary<EquipmentSlot, EquipmentContainer>();
        if (equippedItemStrings == null)
            equippedItemStrings = new Dictionary<EquipmentSlot, string>();

        EquipmentContainer previousItem = GetEquippedItem(slot);
        if (item != null)
        {
            if (equippedItems.ContainsKey(slot))
                equippedItems[slot] = item;
            else
                equippedItems.Add(slot, item);
            if (equippedItemStrings.ContainsKey(slot))
                equippedItemStrings[slot] = item.name;
            else
                equippedItemStrings.Add(slot, item.name);
        }
        else
        {
            if (equippedItems.ContainsKey(slot))
                equippedItems.Remove(slot);
            if (equippedItemStrings.ContainsKey(slot))
                equippedItemStrings.Remove(slot);
        }
        return previousItem;
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
        equippedCombatSkillStrings[index] = combatSkill != null ? combatSkill.name : "";
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
	public int CurrentLevel;
	public float CurrentExp;
	private float requiredExp;

	public int BaseHealth;
	public int CurrentMaxHealth;
    public int CurrentHealth;

    public int BaseAttack;
    public int CurrentAttack;

    public int BaseDefense;
    public int CurrentDefense;

    public int BaseSpeed;
    public int CurrentSpeed;
    public int CurrentInitiative;
	
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
        return Name + ": " + BaseHealth + " / " + CurrentHealth + " HP | "
            + CurrentAttack + " Atk | " + CurrentDefense + " Def | " + CurrentSpeed + " Spd";
    }

    public string Stats()
    {
		StringBuilder stringBuilder = new StringBuilder();
		//throw new System.Exception("Yannick, los mach hier undso!");
        return "Stats:\n HP : " + BaseHealth + "/" + CurrentHealth + "\n Atk  : " + CurrentAttack + "\n Def : " + CurrentDefense + "\n Spd : " + CurrentSpeed;
    }

	public void AddExp(int exp)
	{
		CurrentExp += exp;
		if (CurrentExp >= requiredExp) SetLevel(CurrentLevel + 1);
	}

	public void SetLevel(int level)
	{
		CurrentLevel = level;

		requiredExp = CharDataContainer.baseExpRequirement * CurrentLevel * CharDataContainer.ExpRequirementGrowth;
		if (CurrentExp >= requiredExp)
		{
			SetLevel(CurrentLevel + 1);
			return;
		}

		BaseHealth = CharDataContainer.BaseHealth + (int)(CharDataContainer.HealthGrowth * (CurrentLevel - 1));
		BaseAttack = CharDataContainer.BaseAttack + (int)(CharDataContainer.AttackGrowth * (CurrentLevel - 1));
		BaseDefense = CharDataContainer.BaseDefense + (int)(CharDataContainer.DefenseGrowth * (CurrentLevel - 1));
		BaseSpeed = CharDataContainer.BaseSpeed + (int)(CharDataContainer.SpeedGrowth * (CurrentLevel - 1));

		CurrentHealth = BaseHealth;
		CurrentMaxHealth = BaseHealth;
		CurrentAttack = BaseAttack;
		CurrentDefense = BaseDefense;
		CurrentSpeed = BaseSpeed;

		//foreach (CombatSkill skill in currentSkillCooldowns.Keys)
		//	currentSkillCooldowns[skill] = 0;
		//currentSkillCooldowns.Clear();
		//while (currentCombatEffects.activeCombatEffectElements.Count > 0)
		//	currentCombatEffects.RemoveCombatEffect(currentCombatEffects.activeCombatEffectElements[0]);

	}

    public Entity(Character charDataContainer)
    {
        Name = charDataContainer.name;

        CharacterType = charDataContainer.CharacterType;

        entityType = charDataContainer.name;
        this.charDataContainer = charDataContainer;
		
		CurrentLevel = 1;
		CurrentExp = 0;
		requiredExp = CharDataContainer.baseExpRequirement;

		equippedCombatSkillStrings = new string[4];
		ResetCombatSkills();

        equippedItems = new Dictionary<EquipmentSlot, EquipmentContainer>();
        equippedItemStrings = new Dictionary<EquipmentSlot, string>();

        BaseHealth = charDataContainer.BaseHealth;
		CurrentMaxHealth = BaseHealth;
        CurrentHealth = BaseHealth;

        BaseAttack = charDataContainer.BaseAttack;
        CurrentAttack = BaseAttack;

        BaseDefense = charDataContainer.BaseDefense;
        CurrentDefense = BaseDefense;

        BaseSpeed = charDataContainer.BaseSpeed;
        CurrentSpeed = BaseSpeed;
        CurrentInitiative = 0;
    }
}
