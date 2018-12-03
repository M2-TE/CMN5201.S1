using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public CombatPanel CombatPanel;

    private Entity[] playerTeam, opposingTeam;
    
    private void Start()
    {

    }

    public void StartCombat(Entity[] playerTeam, Entity[] opposingTeam)
    {
        this.playerTeam = playerTeam;
        this.opposingTeam = opposingTeam;

        SetPortraits();
        UpdateSkillIcons();
    }

    private void SetPortraits()
    {
        for (int i = 0; i < CombatPanel.PlayerTeamPortraits.Length; i++)
        {
            bool check = playerTeam[i] != null;
            CombatPanel.PlayerTeamPortraits[i].enabled = check;
            CombatPanel.PlayerTeamPortraits[i].sprite = check ? playerTeam[i].CharDataContainer.Portrait : null;
        }
        for (int i = 0; i < CombatPanel.OpposingTeamPortraits.Length; i++)
        {
            bool check = opposingTeam[i] != null;
            CombatPanel.OpposingTeamPortraits[i].enabled = check;
            CombatPanel.OpposingTeamPortraits[i].sprite = check ? opposingTeam[i].CharDataContainer.Portrait : null;
        }
    }

    private void UpdateSkillIcons()
    {
        for(int i = 0; i < CombatPanel.TeamSkillButtons.Length; i++)
        {
            if (playerTeam[0].EquippedCombatSkills.Length > i)
                CombatPanel.TeamSkillButtons[i].sprite = playerTeam[0].EquippedCombatSkills[i].SkillIcon;
        }
    }
}
