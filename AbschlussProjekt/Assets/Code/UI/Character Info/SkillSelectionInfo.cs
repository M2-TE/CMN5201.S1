using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSelectionInfo : SkillInfo, IDragHandler, IDropHandler
{
    [SerializeField]
    private TextMeshProUGUI skillName;

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
        if(skill != null)
        {

        }
        Debug.Log("DRAG");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("DROP");
    }

}
