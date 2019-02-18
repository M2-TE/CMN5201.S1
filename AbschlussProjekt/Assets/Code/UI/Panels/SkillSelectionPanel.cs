using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillSelectionPanel : UIPanel
{
    #region UIConnectors
    public TextMeshProUGUI characterSkillInfo;
    public TextMeshProUGUI[] RowLevelRequirements;
    public SkillInfo[] characterSkills;
    public SkillSelectionInfo[] possibleSkill;
    #endregion
    public CharacterInfoPanel characterInfoPanel;

    public SkillSelectionManager skillSelectionManager;

    public Entity CurrentEntity;

    public CombatSkill DraggedSkill;

    private int rows;
    private int columns = 4;

    protected override void Awake()
    {
        skillSelectionManager = AssetManager.Instance.GetManager<SkillSelectionManager>() ?? new SkillSelectionManager();
        skillSelectionManager.SkillSelectionPanel = this;
        base.Awake();
    }

    public void ToggleVisibility(bool visibleState, CharacterInfoPanel characterInfoPanel)
    {
        this.characterInfoPanel = characterInfoPanel;
        ToggleVisibility(visibleState);
        if (visibleState)
            DisplaySkillTree();
    }

    public void DisplaySkillTree()
    {
        rows = RowLevelRequirements.Length;
        for (int i = 0; i < rows; i++)
        {
            DisplaySkillTreeRow(i);
            RowLevelRequirements[i].text = CurrentEntity.SkillLevelRequirements[i].ToString();
        }
    }

    private void DisplaySkillTreeRow(int row)
    {
        for (int i = 0; i < columns; i++)
        {
            possibleSkill[row * columns + i].SetUI(SelectItem(row, i));
        }
    }

    private CombatSkill SelectItem(int row, int column)
    {
        CombatSkill outSkill = null;
        int foundSkills = 0;
        for (int i = 0; i < CurrentEntity.CharDataContainer.FullSkillPool.Length; i++)
        {
            if (CurrentEntity.CharDataContainer.FullSkillPool[i].LevelRequirement == CurrentEntity.SkillLevelRequirements[row])
            {
                if (foundSkills == column)
                    outSkill = CurrentEntity.CharDataContainer.FullSkillPool[i];
                foundSkills++;
            }
        }

        return outSkill;
    }

    public void DisplayEquippedSkills()
    {
        for (int i = 0; i < CurrentEntity.EquippedCombatSkills.Length; i++)
        {
            if(CurrentEntity.EquippedCombatSkills[i] != null)
                characterSkills[i].SetUI(CurrentEntity.EquippedCombatSkills[i].SkillIcon,CurrentEntity.EquippedCombatSkills[i].SkillDescription,characterInfoPanel);
            else
                characterSkills[i].SetUI(null, "", characterInfoPanel);
        }
        characterInfoPanel.DisplaySkills();
    }

    public void DisplaySkillInfo(string description)
    {
        if (description != "" && description != null)
            characterSkillInfo.text = "Skill Description:\n" + description;
    }

    public void ClearSkillInfo()
    {
        characterSkillInfo.text = "";
    }

    public void EquipSkill(int position)
    {
        Debug.Log("Equip "+ DraggedSkill.name +" at: " + position);
        for (int i = 0; i < CurrentEntity.EquippedCombatSkills.Length; i++)
        {
            if (CurrentEntity.EquippedCombatSkills[i] != null && CurrentEntity.EquippedCombatSkills[i].name.Equals(DraggedSkill.name))
                CurrentEntity.SetCombatSkill(i, null);
        }
        CurrentEntity.SetCombatSkill(position, DraggedSkill);
        DisplayEquippedSkills();
    }
}
