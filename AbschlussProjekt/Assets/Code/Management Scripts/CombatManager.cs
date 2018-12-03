using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public CombatPanel CombatPanel;

    private Character[] playerTeam, opposingTeam;
    
    private void Start()
    {
        PlayableCharacter chara = AssetManager.Instance.Characters.LoadAsset<PlayableCharacter>("Gunwoman");
        PlayableCharacter oppChara = AssetManager.Instance.Characters.LoadAsset<PlayableCharacter>("Mage");
        StartCombat(new Character[] { chara, chara, chara, chara } , new Character[] { oppChara, chara, oppChara, chara });
    }

    public void StartCombat(Character[] playerTeam, Character[] opposingTeam)
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
            CombatPanel.playerTeamPortraits[i].sprite = check ? playerTeam[i].Portrait : null;
        }
        for (int i = 0; i < CombatPanel.opposingTeamPortraits.Length; i++)
        {
            bool check = opposingTeam[i] != null;
            CombatPanel.opposingTeamPortraits[i].enabled = check;
            CombatPanel.opposingTeamPortraits[i].sprite = check ? opposingTeam[i].Portrait : null;
        }
    }
}
