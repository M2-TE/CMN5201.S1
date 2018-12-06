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
    
    private int? m_currentlySelectedSkill;
	private int? currentlySelectedSkill
	{
		get { return m_currentlySelectedSkill; }
		set
		{
			// deselect previous skill if one was selected
			if (m_currentlySelectedSkill != null) CombatPanel.TeamSkillButtons[(int)currentlySelectedSkill].transform.GetChild(2).gameObject.SetActive(false);
			m_currentlySelectedSkill = value;
			if(value != null) CombatPanel.TeamSkillButtons[(int)currentlySelectedSkill].transform.GetChild(2).gameObject.SetActive(true);
		}
	}

	private LayerMask clickableLayers;

	#region User Input
	private void Start()
	{
		clickableLayers = AssetManager.Instance.Settings.LoadAsset<MiscSettings>("Misc Settings").clickableLayers;
	}

	private void Update()
	{
		CheckForUserInput();
	}

	private void CheckForUserInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(AssetManager.Instance.MainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, clickableLayers);
			if (hit.collider != null) OnCharacterSelect(hit.transform.GetComponent<Proxy>().CombatPosition);
		}
	}
	#endregion

	public void StartCombat(Entity[] playerTeam, Entity[] opposingTeam)
    {
        combatants = new Entity[2, (playerTeam.Length > opposingTeam.Length) ? playerTeam.Length : opposingTeam.Length];
        proxies = new GameObject[2, combatants.GetLength(1)];
        for(int x = 0; x < combatants.GetLength(0); x++)
			for (int y = 0; y < combatants.GetLength(1); y++)
				combatants[x, y] = (x == 0) ? playerTeam[y] : opposingTeam[y];

		SetPortraits();
        InitializeEntities();
        InstantiateProxyPrefabs();

        // initiate combat loop
        LaunchNextTurn();
    }

    private void LaunchNextTurn()
    {
        if (upcomingTurns.Count == 0) ProgressInits();
		
        HandleCurrentTurn();
    }

    #region Single Calls
    private void SetPortraits()
    {
        for (int x = 0; x < combatants.GetLength(0); x++)
        {
            for (int y = 0; y < combatants.GetLength(1); y++)
            {
                bool check = combatants[x, y] != null;
                CombatPanel.TeamPortraits[x, y].enabled = check;
				CombatPanel.TeamPortraits[x, y].sprite = check ? combatants[x, y].CharDataContainer.Portrait : null;
            }
        }
    }

    private void InitializeEntities()
    {
		Entity tempEntity;
        for (int x = 0; x < combatants.GetLength(0); x++)
		{
			for (int y = 0; y < combatants.GetLength(1); y++)
			{
				tempEntity = combatants[x, y];
				if (tempEntity != null)
				{
					tempEntity.currentInitiative = 0;
					tempEntity.currentCombatPosition = new Vector2Int(x, y);
				}
			}
		}
    }

    private void InstantiateProxyPrefabs()
    {
        for (int x = 0; x < combatants.GetLength(0); x++)
        {
            for (int y = 0; y < combatants.GetLength(1); y++)
            {
				GameObject combatPrefab = CombatPanel.CharacterPositions[x, y];
                if(combatants[x, y] == null) continue;
				
				// spawn proxy (proxy => represents an entity in the world)
                GameObject tempProxy = Instantiate(combatants[x, y].CharDataContainer.Prefab);
				tempProxy.GetComponent<Proxy>().CombatPosition = new Vector2Int(x, y);

                Vector3 proxyPos = AssetManager.Instance.MainCam.ScreenToWorldPoint(combatPrefab.transform.position);
				if (x == 1) tempProxy.transform.localScale = new Vector3(-1f, 1f, 1f); // flip if its an opponent proxy
				proxyPos.z = 0f;
                tempProxy.transform.position = proxyPos;
                proxies[x, y] = tempProxy;
            }
        }
    }
    #endregion

    #region Combat Loop Calls
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
		// Repeat until an entity gains a turn
		if (upcomingTurns.Count == 0) ProgressInits();

		// Sort all upcoming turns by total initiative (entities with higher init come first)
		else upcomingTurns.Sort((first, second) 
			=> combatants[first.x, first.y].currentInitiative.CompareTo
			(combatants[second.x, second.y].currentInitiative));
    }

    private void UpdateSkillIcons()
    {
        for (int i = 0; i < CombatPanel.TeamSkillButtons.Length; i++)
        {
            CombatPanel.TeamSkillButtons[i].gameObject.SetActive(upcomingTurns[0].x == 0);

			Entity currentlyActiveEntity = combatants[upcomingTurns[0].x, upcomingTurns[0].y];
            CombatPanel.TeamSkillButtons[i].sprite = (currentlyActiveEntity.EquippedCombatSkills.Length > i)
                ? CombatPanel.TeamSkillButtons[i].sprite = currentlyActiveEntity.EquippedCombatSkills[i].SkillIcon
                : null;
        }
    }

	#region Turn Management
	private void HandleCurrentTurn()
    {
		// change this to choose the last-selected or first skill of that character
		currentlySelectedSkill = null; 
		UpdateSkillIcons();

		if (upcomingTurns[0].x == 1) ControlOpponentTurn();
		// else wait for player input;
	}

	private void ControlOpponentTurn()
	{
		EndTurn();
		LaunchNextTurn();
	}

	private void EndTurn()
	{
		upcomingTurns.RemoveAt(0);
	}
	#endregion

	private void UseSkill(Vector2Int mainTarget)
	{

	}
	#endregion

	#region User Interaction
	private void OnCharacterSelect(Vector2Int characterPos)
	{
		Debug.Log(characterPos);
	}

	public void OnSkillSelect(int skillID)
    {
        if (upcomingTurns[0].x == 1 || combatants[upcomingTurns[0].x, upcomingTurns[0].y].EquippedCombatSkills.Length <= skillID) return;

		currentlySelectedSkill = skillID;
    }
	#endregion
}
