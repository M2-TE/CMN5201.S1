using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image skillImage;
    protected string skillDescription;
    protected CharacterInfoPanel characterInfoPanel;

    public void Start()
    {
        characterInfoPanel = AssetManager.Instance.GetManager<CharacterInfoManager>().CharacterInfoPanel;
    }

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

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (characterInfoPanel == null)
            characterInfoPanel = AssetManager.Instance.GetManager<CharacterInfoManager>().CharacterInfoPanel;
        characterInfoPanel.DisplaySkillInfo(skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (characterInfoPanel == null)
            characterInfoPanel = AssetManager.Instance.GetManager<CharacterInfoManager>().CharacterInfoPanel;
        characterInfoPanel.ClearSkillInfo();
    }
}
