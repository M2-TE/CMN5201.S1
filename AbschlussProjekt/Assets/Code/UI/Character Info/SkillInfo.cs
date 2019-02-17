using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image skillImage;
    private string skillDescription;
    protected CharacterInfoPanel characterInfoPanel;

    public void SetUI(Sprite sprite, string skillDescription, CharacterInfoPanel characterInfoPanel)
    {
        if(sprite != null)
        {
            skillImage.sprite = sprite;
            skillImage.color = Color.white;
            this.skillDescription = skillDescription;
        }
        else
        {
            skillImage.color = Vector4.zero;
            skillDescription = "";
        }
        this.characterInfoPanel = characterInfoPanel;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        characterInfoPanel.DisplaySkillInfo(skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        characterInfoPanel.ClearSkillInfo();
    }
}
