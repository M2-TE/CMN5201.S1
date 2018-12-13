using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatManager : MonoBehaviour
{
	#region Variables
	// change this \/ to DataContainer ref
	[SerializeField] private CombatPanel combatPanel;
	[SerializeField] private EventSystem eventSystem;

    // first dimension for team (0 player, 1 opponent), second dimension for specific character
    private Entity[,] combatants;
    private GameObject[,] proxies;
	private bool[,] selectableTargets;
	private List<Vector2Int> upcomingTurns = new List<Vector2Int>();
    private int totalTurns = 0;
	
	private int m_currentlySelectedSkill;
	private int currentlySelectedSkill
	{
		get { return m_currentlySelectedSkill; }
		set
		{
			// deselect previous skill if one was selected
			combatPanel.TeamSkillButtons[m_currentlySelectedSkill].transform.GetChild(2).gameObject.SetActive(false);
			m_currentlySelectedSkill = value;
			combatPanel.TeamSkillButtons[m_currentlySelectedSkill].transform.GetChild(2).gameObject.SetActive(true);
		}
	}

	private LayerMask clickableLayers;
	private float healthbarAdjustmentSpeed = 10f;
	#endregion

	#region Combat Startup
	private void Start()
	{
		clickableLayers = AssetManager.Instance.Settings.LoadAsset<MiscSettings>("Misc Settings").clickableLayers;
	}
	
	public void StartCombat(Entity[] playerTeam, Entity[] opposingTeam)
	{
		combatants = new Entity[2, (playerTeam.Length > opposingTeam.Length) ? playerTeam.Length : opposingTeam.Length];
		proxies = new GameObject[2, combatants.GetLength(1)];
		selectableTargets = new bool[2, combatants.GetLength(1)];

		for (int x = 0; x < combatants.GetLength(0); x++)
			for (int y = 0; y < combatants.GetLength(1); y++)
				combatants[x, y] = (x == 0) ? playerTeam[y] : opposingTeam[y];

		InitializeUI();
		InitializeEntities();
		InstantiateProxyPrefabs();

		// initiate combat loop
		LaunchNextTurn();
	}
	#endregion

	#region User Input, Health Bar Updates (TODO: this update doesnt need to be called every frame)
	private void Update()
	{
		CheckForUserInput();
		UpdateHealthBars();
	}

	private void CheckForUserInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(AssetManager.Instance.MainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, clickableLayers);
			if (hit.collider != null) OnCharacterSelect(hit.transform.GetComponent<Proxy>().CombatPosition);
		}
	}

	private void OnCharacterSelect(Vector2Int characterPos)
	{
		if (!GetButtonsEnabled() || GetEntity(characterPos).currentHealth == 0) return;

		SetButtonsEnabled(false);
		UseCombatSkill(characterPos);
	}
	#endregion

	#region Single Calls
	private void InitializeUI()
    {
		UpdatePortraits();
		//UpdateHealthBars();
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
				GameObject combatPrefab = combatPanel.CharacterPositions[x, y];
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
	#region Turn Management
	private void LaunchNextTurn()
	{
		if (upcomingTurns.Count == 0) ProgressInits();

		HandleCurrentTurn();
	}

	private void HandleCurrentTurn()
	{
		// change this to choose the last-selected or first skill of that character
		currentlySelectedSkill = 0;
		UpdateSkillIcons();

		if (upcomingTurns[0].x == 1)
		{
			// -> Opponents Turn
			SetButtonsEnabled(false);
			ControlOpponentTurn();
		}
		else
		{
			// -> Players Turn
			SetButtonsEnabled(true);
			OnSkillSelect(currentlySelectedSkill);
			combatPanel.EventSystem.SetSelectedGameObject(null);
			combatPanel.EventSystem.SetSelectedGameObject(combatPanel.TeamSkillButtons[0].gameObject);
		}

		UpdateSelectableTargets();
	}
	
	private void EndTurn()
	{
		GetEntity(upcomingTurns[0]).currentInitiative = 0;
		upcomingTurns.RemoveAt(0);

		// start next turn if combat is still ongoing
		if(!CheckForCombatEnd()) LaunchNextTurn();
	}

	private void ControlOpponentTurn()
	{
		// select AI's skill
		currentlySelectedSkill = 0;

		List<Vector2Int> validTargets = new List<Vector2Int>();
		for (int i = 0; i < combatants.GetLength(1); i++)
			if(combatants[0, i] != null && combatants[0, i].currentHealth != 0)
				validTargets.Add(combatants[0, i].currentCombatPosition);

		if(validTargets.Count > 0)
		{
			Vector2Int targetPos;
			targetPos = validTargets[Random.Range(0, validTargets.Count)];
			UseCombatSkill(targetPos);
		}
		else
			EndTurn(); // -> do nothing and end turn if no valid target was found
	}

	private void ProgressInits()
	{
		for (int x = 0; x < combatants.GetLength(0); x++)
		{
			for (int y = 0; y < combatants.GetLength(1); y++)
			{
				Entity tempEntity = combatants[x, y];
				if (tempEntity == null || tempEntity.currentHealth == 0) continue;

				tempEntity.currentInitiative += tempEntity.currentSpeed;
				if (tempEntity.currentInitiative >= 100)
					upcomingTurns.Add(new Vector2Int(x, y));
			}
		}
		// Repeat until an entity gains a turn
		if (upcomingTurns.Count == 0) ProgressInits();

		// Sort all upcoming turns by total initiative (entities with higher init come first)
		else upcomingTurns.Sort((first, second)
				=> GetEntity(second).currentInitiative.CompareTo(GetEntity(first).currentInitiative));
	}
	#endregion

	#region UI Updates
	private void UpdateSkillIcons()
    {
        for (int i = 0; i < combatPanel.TeamSkillButtons.Length; i++)
        {
            combatPanel.TeamSkillButtons[i].gameObject.SetActive(upcomingTurns[0].x == 0);

			Entity currentlyActiveEntity = combatants[upcomingTurns[0].x, upcomingTurns[0].y];
			combatPanel.TeamSkillButtons[i].sprite = (currentlyActiveEntity.EquippedCombatSkills[i] == null) 
				? null 
				: currentlyActiveEntity.EquippedCombatSkills[i].SkillIcon;
        }
    }

	private void UpdatePortraits()
	{
		for (int x = 0; x < combatants.GetLength(0); x++)
		{
			for (int y = 0; y < combatants.GetLength(1); y++)
			{
				bool check = combatants[x, y] != null;
				combatPanel.TeamPortraits[x, y].enabled = check;
				combatPanel.TeamPortraits[x, y].sprite = check ? combatants[x, y].CharDataContainer.Portrait : null;
			}
		}
	}

	private void UpdateHealthBars()
	{
		for (int x = 0; x < combatants.GetLength(0); x++)
		{
			for (int y = 0; y < combatants.GetLength(1); y++)
			{
				bool check = combatants[x, y] != null;
				combatPanel.HealthBars[x, y].gameObject.SetActive(check);
				if (check)
				{
					combatPanel.HealthBars[x, y].maxValue = combatants[x, y].currentMaxHealth;
					combatPanel.HealthBars[x, y].value = 
						Mathf.MoveTowards
						(combatPanel.HealthBars[x, y].value,
						combatants[x, y].currentHealth,
						healthbarAdjustmentSpeed * Time.deltaTime);
				}
			}
		}
	}

	private void UpdateSelectableTargets()
	{
		CombatSkill skill = GetEntity(upcomingTurns[0]).EquippedCombatSkills[currentlySelectedSkill];
		for (int x = 0; x < combatPanel.SelectableIndicators.GetLength(0); x++)
		{
			for (int y = 0; y < combatPanel.SelectableIndicators.GetLength(1); y++)
			{
				// dont show indicator if: 1: Its an enemy's turn. 2: The spot is empty. 3: The character is dead (and no revive skill is in use)
				Vector2Int indicPos = new Vector2Int(x, y);
				if (upcomingTurns[0].x == 1 
					|| GetEntity(indicPos) == null 
					|| GetEntity(indicPos).currentHealth == 0) selectableTargets[x, y] = false;
				else
				{
					// player team indicator
					if (x == 0)
					{
						if (y == upcomingTurns[0].y) selectableTargets[x, y] = skill.CanHitSelf;
						else selectableTargets[x, y] = skill.CanHitAllies && (Mathf.Abs(upcomingTurns[0].y - y) <= skill.Range);
					}
					//enemy team indicator
					else
					{
						selectableTargets[x, y] = skill.CanHitEnemies && (upcomingTurns[0].y + y + 1 <= skill.Range);
					}
				}

				combatPanel.SelectableIndicators[x, y].enabled = selectableTargets[x, y];
			}
		}
	}
	#endregion

	#region Character Action Handling
	private void UseCombatSkill(Vector2Int mainTarget)
	{
		GetProxy(upcomingTurns[0]).GetComponent<Animator>().SetTrigger("Attack");

		// spawn attack fx on enemies with a certain delay (after triggering atk anim)
		Vector2Int[] targets = new Vector2Int[] { mainTarget };

		StartCoroutine(LaunchAttack(GetEntity(upcomingTurns[0]).CharDataContainer.attackAnimDelay, targets));
	}

	private IEnumerator LaunchAttack(float attackDelay, Vector2Int[] targets)
	{
		yield return new WaitForSeconds(attackDelay);

		CombatSkill skill = GetEntity(upcomingTurns[0]).EquippedCombatSkills[currentlySelectedSkill];
		GameObject[] proxyArr = GetProxies(targets);
		Transform effectTransform = null;

		for(int i = 0; i < targets.Length; i++)
		{
			effectTransform = Instantiate(skill.FxPrefab, proxyArr[i].transform).transform;
			ApplyCombatSkill(upcomingTurns[0], targets[i], skill);
		}
		
		// wait until the dmg fx has faded
		while (effectTransform != null) yield return null;

		// check for targets death
		for (int i = 0; i < targets.Length; i++)
			if (GetEntity(targets[i]).currentHealth == 0) TriggerDeath(GetEntity(targets[i]).currentCombatPosition);
	
		EndTurn();
	}

	private void ApplyCombatSkill(Vector2Int caster, Vector2Int target, CombatSkill skill)
	{
		int actualDamage = (int)Mathf.Max(0f, GetEntity(caster).currentAttack * skill.AttackMultiplier - GetEntity(target).currentDefense);
		ApplyDamage(target, actualDamage);
	}

	private void ApplyDamage(Vector2Int target, int trueDamage)
	{
		GetEntity(target).currentHealth = Mathf.Max(0, GetEntity(target).currentHealth - trueDamage);
	}

	private void TriggerDeath(Vector2Int dyingCharPos)
	{
		upcomingTurns.Remove(dyingCharPos); // TODO: check if this ever throws an exception
		GetEntity(dyingCharPos).currentInitiative = 0;

		GameObject proxy = GetProxy(dyingCharPos); 
		proxy.GetComponent<Animator>().enabled = false; // pause anim
		proxy.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .25f); // slight transparency

		// TODO edit portrait
	}
	#endregion

	#region Combat End Handling
	private bool CheckForCombatEnd()
	{
		bool playerAlive = false;
		bool opponentAlive = false;

		for(int x = 0; x < combatants.GetLength(0); x++)
		{
			for(int y = 0; y < combatants.GetLength(1); y++)
			{
				if (combatants[x, y] != null && combatants[x, y].currentHealth != 0)
				{
					if (x == 0) playerAlive = true;
					else opponentAlive = true;
				}
			}
		}

		if (playerAlive && opponentAlive) return false;

		// playerWon == null means a draw ocurred
		bool? playerWon = null;
		if (!playerAlive && opponentAlive) playerWon = false;
		if (playerAlive && !opponentAlive) playerWon = true;
		EndCombatPhase(playerWon);

		// tell the combat loop that its love relationship is done
		return true;
	}

	private void EndCombatPhase(bool? playerWon)
	{
		Debug.Log("Player Victory: " + playerWon);
	}
	#endregion
	#endregion

	#region Utility Methods
	private Entity GetEntity(Vector2Int entityPos)
	{
		return combatants[entityPos.x, entityPos.y];
	}
	private Entity[] GetEntities(Vector2Int[] entityPosArr)
	{
		Entity[] entityArr = new Entity[entityPosArr.Length];
		for (int i = 0; i < entityPosArr.Length; i++)
			entityArr[i] = combatants[entityPosArr[i].x, entityPosArr[i].y];
		return entityArr;
	}

	private GameObject GetProxy(Vector2Int proxyPos)
	{
		return proxies[proxyPos.x, proxyPos.y];
	}
	private GameObject[] GetProxies(Vector2Int[] proxPosArr)
	{
		GameObject[] proxieArr = new GameObject[proxPosArr.Length];
		for (int i = 0; i < proxPosArr.Length; i++)
			proxieArr[i] = proxies[proxPosArr[i].x, proxPosArr[i].y];
		return proxieArr;
	}

	private void SetButtonsEnabled(bool enableState)
	{
		for (int i = 0; i < combatPanel.TeamSkillButtons.Length; i++)
			combatPanel.TeamSkillButtons[i].gameObject.SetActive(enableState);
	}
	private bool GetButtonsEnabled()
	{
		bool active = false;

		for (int i = 0; i < combatPanel.TeamSkillButtons.Length; i++)
			if (combatPanel.TeamSkillButtons[i].gameObject.activeSelf) active = true;

		return active;
	}
	#endregion

	#region Public Button Methods
	public void OnSkillSelect(int skillID)
    {
        if (upcomingTurns[0].x == 1 || GetEntity(upcomingTurns[0]).EquippedCombatSkills[skillID] == null) return;

		currentlySelectedSkill = skillID;

		UpdateSelectableTargets();
    }
	#endregion
}