using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    // change this \/ to DataContainer ref
    public CombatPanel CombatPanel;

    private Entity[] playerTeam, opposingTeam;
    private GameObject[] playerProxies, opposingProxies;

    private Queue<Entity> upcomingTurns = new Queue<Entity>();
    private int totalTurns = 0;

    private bool userInputGiven;
    private bool combatOngoing;
    private bool playerTeamTurn;
    private int currentlySelectedSkill;
    private int currentlyActiveEntity;

    private bool debugActive = true;
    // DEBUGGING //
    private void Update()
    {
        if (!debugActive) return;

        if(Input.GetKeyDown(KeyCode.E))
        {
            playerTeamTurn = (playerTeamTurn) ? false : true;
            UpdateSkillIcons();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentlyActiveEntity--;
            UpdateSkillIcons();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            currentlyActiveEntity++;
            UpdateSkillIcons();
        }
        if (Input.GetKeyDown(KeyCode.Space))
            userInputGiven = true;
    }

    public void StartCombat(Entity[] playerTeam, Entity[] opposingTeam)
    {
        this.playerTeam = playerTeam;
        this.opposingTeam = opposingTeam;

        SetPortraits();
        ResetInitiatives();
        InstantiateProxyPrefabs();

        StartCoroutine(CombatLoop());
    }

    // this is shit
    private IEnumerator CombatLoop()
    {
        combatOngoing = true;
        while (combatOngoing)
        {
            if (upcomingTurns.Count == 0) CalculateInitiatives();
            else HandleTurn();

            UpdateSkillIcons();

            yield return null;
        }
    }

    #region Single Calls
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

    private void ResetInitiatives()
    {
        for (int i = 0; i < playerTeam.Length; i++)
            if(playerTeam[i] != null) playerTeam[i].currentInitiative = 0;

        for (int i = 0; i < opposingTeam.Length; i++)
            if(opposingTeam[i] != null) opposingTeam[i].currentInitiative = 0;
    }

    private void InstantiateProxyPrefabs()
    {
        playerProxies = new GameObject[playerTeam.Length];
        for (int i = 0; i < playerTeam.Length; i++)
        {
            if (playerTeam[i] == null)
            {
                CombatPanel.PlayerCombatPrefabs[i].gameObject.SetActive(false);
                continue;
            }

            CombatPanel.OpposingCombatPrefabs[i].gameObject.SetActive(true);
            GameObject tempProxy = Instantiate(playerTeam[i].CharDataContainer.Prefab);
            Vector3 proxyPos = AssetManager.Instance.MainCam.ScreenToWorldPoint(CombatPanel.PlayerCombatPrefabs[i].transform.position);
            proxyPos.z = 0f;
            tempProxy.transform.position = proxyPos;
            playerProxies[i] = tempProxy;
        }

        opposingProxies = new GameObject[opposingTeam.Length];
        for (int i = 0; i < opposingTeam.Length; i++)
        {
            if (opposingTeam[i] == null)
            {
                CombatPanel.OpposingCombatPrefabs[i].gameObject.SetActive(false);
                continue;
            }

            CombatPanel.OpposingCombatPrefabs[i].gameObject.SetActive(true);
            GameObject tempProxy = Instantiate(opposingTeam[i].CharDataContainer.Prefab);
            Vector3 proxyPos = AssetManager.Instance.MainCam.ScreenToWorldPoint(CombatPanel.OpposingCombatPrefabs[i].transform.position);
            proxyPos.z = 0f;
            tempProxy.transform.localScale = new Vector3(-1f, 1f, 1f);
            tempProxy.transform.position = proxyPos;
            opposingProxies[i] = tempProxy;
        }

    }
    #endregion

    #region Repeated Calls
    private void CalculateInitiatives()
    {
        Entity tempEntity;
        for(int i = 0; i < playerTeam.Length; i++)
        {
            tempEntity = playerTeam[i];
            if (tempEntity == null) continue;

            tempEntity.currentInitiative += tempEntity.currentSpeed;
            if (tempEntity.currentInitiative <= 100)
            {
                upcomingTurns.Enqueue(tempEntity);
                tempEntity.currentInitiative = 0;
            }
        }

        for(int i = 0; i < opposingTeam.Length; i++)
        {
            tempEntity = opposingTeam[i];
            if (tempEntity == null) continue;

            tempEntity.currentInitiative += tempEntity.currentSpeed;
            if (tempEntity.currentInitiative <= 100)
            {
                upcomingTurns.Enqueue(tempEntity);
                tempEntity.currentInitiative = 0;
            }
        }
    }

    private void HandleTurn()
    {
        totalTurns++;
        Entity tempEntity = upcomingTurns.Dequeue();
        Debug.Log(tempEntity.CharDataContainer);
    }

    private void UpdateSkillIcons()
    {
        for (int i = 0; i < CombatPanel.TeamSkillButtons.Length; i++)
        {
            CombatPanel.TeamSkillButtons[i].gameObject.SetActive(playerTeamTurn);

            CombatPanel.TeamSkillButtons[i].sprite = (playerTeam[currentlyActiveEntity].EquippedCombatSkills.Length > i)
                ? CombatPanel.TeamSkillButtons[i].sprite = playerTeam[currentlyActiveEntity].EquippedCombatSkills[i].SkillIcon
                : null;
        }
    }
    #endregion

    public void OnSkillSelect(int skillID)
    {
        if (!playerTeamTurn || playerTeam[currentlyActiveEntity].EquippedCombatSkills.Length <= skillID) return;

        CombatPanel.TeamSkillButtons[currentlySelectedSkill].transform.GetChild(2).gameObject.SetActive(false);
        currentlySelectedSkill = skillID;
        CombatPanel.TeamSkillButtons[currentlySelectedSkill].transform.GetChild(2).gameObject.SetActive(true);
    }
}
