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
    }

    private void SetPortraits()
    {
        for (int i = 0; i < CombatPanel.playerTeamPortraits.Length; i++)
        {
            bool check = playerTeam[i] != null;
            CombatPanel.playerTeamPortraits[i].enabled = check;
            CombatPanel.playerTeamPortraits[i].sprite = check ? playerTeam[i].CharDataContainer.Portrait : null;
        }
        for (int i = 0; i < CombatPanel.opposingTeamPortraits.Length; i++)
        {
            bool check = opposingTeam[i] != null;
            CombatPanel.opposingTeamPortraits[i].enabled = check;
            CombatPanel.opposingTeamPortraits[i].sprite = check ? opposingTeam[i].CharDataContainer.Portrait : null;
        }
    }
}
