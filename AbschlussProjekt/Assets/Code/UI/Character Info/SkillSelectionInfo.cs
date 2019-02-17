using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSelectionInfo : SkillInfo,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private TextMeshProUGUI skillName;
    [SerializeField]
    private SkillSelectionPanel skillSelectionPanel;
    [SerializeField] [Range(0,3)]
    private int position;

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

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
