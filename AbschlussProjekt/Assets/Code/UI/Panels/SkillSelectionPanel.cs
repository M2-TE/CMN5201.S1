using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillSelectionPanel : UIPanel
{
    #region UIConnectors
    public TextMeshProUGUI characterSkillInfo;
    public SkillInfo[] characterSkills;
    public SkillSelectionInfo[] possibleSkill;
    #endregion
    public CharacterInfoPanel characterInfoPanel;

    public SkillSelectionManager skillSelectionManager;

    public Entity CurrentEntity;

    protected override void Awake()
    {
        skillSelectionManager = AssetManager.Instance.GetManager<SkillSelectionManager>() ?? new SkillSelectionManager();
        skillSelectionManager.SkillSelectionPanel = this;
        base.Awake();
    }

    public void DisplaySkillTree()
    {
        DisplayEquippedSkills();
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

}
