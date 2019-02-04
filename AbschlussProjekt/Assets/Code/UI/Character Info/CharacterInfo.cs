using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    public static Vector4 invisColor = Vector4.zero;
    public static Color opaqueColor = Color.white;

    [SerializeField]
    private TextMeshProUGUI characterName, characterStats, characterSkillInfo;
    [SerializeField]
    private Image characterPortrait;
    [SerializeField]
    private Image[] characterBuffs;
    [SerializeField]
    private SkillInfo[] characterSkills;

    private Entity currentCharacter;

    public void DisplayCharacter(Entity character)
    {
        if (character == null)
            return;
        currentCharacter = character;

        characterName.text = character.Name;
        characterStats.text = character.Stats();

        characterPortrait.sprite = character.CharDataContainer.Portrait;

        SetBuffImageVisibility();

        for (int i = 0; i < characterSkills.Length; i++)
        {
            if (character.EquippedCombatSkills[i] != null)
                characterSkills[i].SetUI(character.EquippedCombatSkills[i].SkillIcon, character.EquippedCombatSkills[i].SkillDescription, this);
            else
                characterSkills[i].SetUI(null, "", this);
        }

        if(character.currentCombatEffects != null)
        {
            for (int i = 0; i < character.currentCombatEffects.activeCombatEffectElements.Count; i++)
            {
                characterBuffs[i].sprite = character.currentCombatEffects.activeCombatEffectElements[i].CombatEffect.EffectSprite;
                characterBuffs[i].color = opaqueColor;
            }
        }
    }

    private void SetBuffImageVisibility()
    {
        for (int i = 0; i < characterBuffs.Length; i++)
        {
            characterBuffs[i].color = invisColor;
        }
    }

    public void DisplaySkillInfo(string description)
    {
        if(description != "" && description != null)
            characterSkillInfo.text = "Skill Description:\n"+ description;
    }

    public void ClearSkillInfo()
    {
        characterSkillInfo.text = "";
    }

}
