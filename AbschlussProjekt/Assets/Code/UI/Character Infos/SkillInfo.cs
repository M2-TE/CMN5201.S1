using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image skillImage;
    private string skillDescription;
    private CharacterInfo manager;

    public void SetUI(Sprite sprite, string skillDescription, CharacterInfo manager)
    {
        if(sprite != null)
        {
            skillImage.sprite = sprite;
            skillImage.color = new Vector4(0, 0, 0, 1);
            this.skillDescription = skillDescription;
        }
        else
        {
            skillImage.color = Vector4.zero;
            skillDescription = "";
        }
        this.manager = manager;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.DisplaySkillInfo(skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.ClearSkillInfo();
    }
}
