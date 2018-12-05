using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    // change this \/ to DataContainer ref
    public CombatPanel CombatPanel;

    // first dimension for team (0 player, 1 opponent), second dimension for specific character
    private Entity[,] combatants;
    private GameObject[,] proxies;
    private List<Vector2Int> upcomingTurns = new List<Vector2Int>();
    private int totalTurns = 0;
    
    private bool combatOngoing;
    // change this \/ (deprecated) read current turn through upcoming turn list
    private bool playerTeamTurn;
    private int currentlySelectedSkill;
    private int currentlyActiveEntity;
    private float turnTime = 5f;

    // DEBUGGING //
    private bool debugActive = false;
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
    }

    public void StartCombat(Entity[] playerTeam, Entity[] opposingTeam)
    {
        combatants = new Entity[2, (playerTeam.Length > opposingTeam.Length) ? playerTeam.Length : opposingTeam.Length];
        proxies = new GameObject[2, combatants.GetLength(1)];
        for(int x = 0; x < combatants.GetLength(0); x++)
        {
            for(int y = 0; y < combatants.GetLength(1); y++)
            {
                combatants[x, y] = (x == 0) ? playerTeam[y] : opposingTeam[y];
            }
        }

        SetPortraits();
        ResetInitiatives();
        InstantiateProxyPrefabs();

        // start game loop
        LaunchNextTurn();
    }
    

    private void LaunchNextTurn()
    {
        if (upcomingTurns.Count == 0) ProgressInits();
        
    }

    #region Single Calls
    private void SetPortraits()
    {
        for (int x = 0; x < combatants.GetLength(0); x++)
        {
            for (int y = 0; y < combatants.GetLength(1); y++)
            {
                bool check = combatants[x, y] != null;
                Image tempPortrait = (x == 0) ? CombatPanel.PlayerTeamPortraits[y] : CombatPanel.OpposingTeamPortraits[y];

                tempPortrait.enabled = check;
                tempPortrait.sprite = check ? combatants[x, y].CharDataContainer.Portrait : null;
            }
        }
    }

    private void ResetInitiatives()
    {
        for (int x = 0; x < combatants.GetLength(0); x++)
            for (int y = 0; y < combatants.GetLength(1); y++)
                if (combatants[x, y] != null) combatants[x, y].currentInitiative = 0;
    }

    private void InstantiateProxyPrefabs()
    {
        for (int x = 0; x < combatants.GetLength(0); x++)
        {
            for (int y = 0; y < combatants.GetLength(1); y++)
            {
                GameObject combatPrefab = (x == 0) ? CombatPanel.PlayerCombatPrefabs[y] : CombatPanel.OpposingCombatPrefabs[y];
                if(combatants[x, y] == null)
                {
                    combatPrefab.gameObject.SetActive(false);
                    continue;
                }

                combatPrefab.gameObject.SetActive(true);

                GameObject tempProxy = Instantiate(combatants[x, y].CharDataContainer.Prefab);
                Vector3 proxyPos = AssetManager.Instance.MainCam.ScreenToWorldPoint(combatPrefab.transform.position);
                proxyPos.z = 0f;
                if(x == 1) tempProxy.transform.localScale = new Vector3(-1f, 1f, 1f);
                tempProxy.transform.position = proxyPos;
                proxies[x, y] = tempProxy;
            }
        }
    }
    #endregion

    #region Repeated Calls
    private void ProgressInits()
    {
        for (int x = 0; x < combatants.GetLength(0); x++)
        {
            for (int y = 0; y < combatants.GetLength(1); y++)
            {
                Entity tempEntity = combatants[x, y];
                if (tempEntity == null) continue;

                tempEntity.currentInitiative += tempEntity.currentSpeed;
                if (tempEntity.currentInitiative >= 100)
                {
                    upcomingTurns.Add(new Vector2Int(x, y));
                    tempEntity.currentInitiative = 0;
                }
            }
        }
        if (upcomingTurns.Count == 0) ProgressInits();
    }

    private void UpdateSkillIcons()
    {
        for (int i = 0; i < CombatPanel.TeamSkillButtons.Length; i++)
        {
            CombatPanel.TeamSkillButtons[i].gameObject.SetActive(playerTeamTurn);

            CombatPanel.TeamSkillButtons[i].sprite = (combatants[0, currentlyActiveEntity].EquippedCombatSkills.Length > i)
                ? CombatPanel.TeamSkillButtons[i].sprite = combatants[0, currentlyActiveEntity].EquippedCombatSkills[i].SkillIcon
                : null;
        }
    }
    #endregion

    private void UseSkill(int skillID, Entity mainTarget)
    {
        Entity caster = combatants[upcomingTurns[0].x, upcomingTurns[0].y];
        CombatSkill usedSkill = caster.EquippedCombatSkills[skillID];
    }

    private IEnumerator NextTurnStarter()
    {
        yield return new WaitForSeconds(turnTime);
        LaunchNextTurn();
    }

    public void OnSkillSelect(int skillID)
    {
        if (!playerTeamTurn || combatants[0, currentlyActiveEntity].EquippedCombatSkills.Length <= skillID) return;

        CombatPanel.TeamSkillButtons[currentlySelectedSkill].transform.GetChild(2).gameObject.SetActive(false);
        currentlySelectedSkill = skillID;
        CombatPanel.TeamSkillButtons[currentlySelectedSkill].transform.GetChild(2).gameObject.SetActive(true);
    }
}
