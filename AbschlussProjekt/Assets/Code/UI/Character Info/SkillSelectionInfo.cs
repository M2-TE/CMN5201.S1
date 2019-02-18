using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSelectionInfo : SkillInfo
{
    [SerializeField]
    private bool equippedSlot = false;
    [SerializeField]
    private TextMeshProUGUI skillName;
    [Range(0,3)]
    public int Position;
    public SkillSelectionPanel skillSelectionPanel;
    [HideInInspector] public Vector3 LocalPosition;

    public CombatSkill skill;

    public void SetUI()
    {
        if(skill != null)
        {
            SetUI(skill.SkillIcon, skill.SkillDescription, characterInfoPanel);
            skillName.text = skill.name;
        }
        else
        {
            SetUI(null, "", characterInfoPanel);
            skillName.text = "";
        }
    }

    public void SetUI(CombatSkill skill)
    {
        this.skill = skill;
        SetUI();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (skillDescription != null)
            base.OnPointerEnter(eventData);
        else if (skill != null)
            characterInfoPanel.DisplaySkillInfo(skill.SkillDescription);
        if(equippedSlot && skillSelectionPanel.DraggedSkill != null)
        {
            skillSelectionPanel.EquipSkill(Position);
        }
    }


}
