using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
	#region Variables
	// change this \/ to DataContainer ref
	[SerializeField] private CombatPanel combatPanel;
	[SerializeField] private EventSystem eventSystem;

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
			if (m_currentlySelectedSkill != null) combatPanel.TeamSkillButtons[(int)currentlySelectedSkill].transform.GetChild(2).gameObject.SetActive(false);
			m_currentlySelectedSkill = value;
			if(value != null) combatPanel.TeamSkillButtons[(int)currentlySelectedSkill].transform.GetChild(2).gameObject.SetActive(true);
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

	#region Update
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
		if (currentlySelectedSkill == null || !GetButtonsEnabled()) return;

		SetButtonsEnabled(false);
		UseSkill(characterPos);
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
		currentlySelectedSkill = null;
		UpdateSkillIcons();

		if (upcomingTurns[0].x == 1)
		{
			SetButtonsEnabled(false);
			ControlOpponentTurn();
		}
		else
		{
			SetButtonsEnabled(true);
			OnSkillSelect(0);
		}
	}
	
	private void EndTurn()
	{
		GetEntity(upcomingTurns[0]).currentInitiative = 0;
		upcomingTurns.RemoveAt(0);
		LaunchNextTurn();
	}

	private void ControlOpponentTurn()
	{
		currentlySelectedSkill = 0;
		UseSkill(new Vector2Int(0, Random.Range(0, combatants.GetLength(0)) + 1));
	}
	#endregion

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
					upcomingTurns.Add(new Vector2Int(x, y));
			}
        }
		// Repeat until an entity gains a turn
		if (upcomingTurns.Count == 0) ProgressInits();

		// Sort all upcoming turns by total initiative (entities with higher init come first)
		else upcomingTurns.Sort((first, second)
				=> GetEntity(second).currentInitiative.CompareTo(GetEntity(first).currentInitiative));
	}

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

	private void UseSkill(Vector2Int mainTarget)
	{
		GetProxy(upcomingTurns[0]).GetComponent<Animator>().SetTrigger("Attack");

		// spawn attack fx on enemies with a certain delay (after triggering atk anim)
		Vector2Int[] targets = new Vector2Int[] { mainTarget };

		StartCoroutine(ApplyFX(GetEntity(upcomingTurns[0]).CharDataContainer.attackAnimDelay, targets));
	}

	private IEnumerator ApplyFX(float attackDelay, Vector2Int[] targets)
	{
		yield return new WaitForSeconds(attackDelay);

		CombatSkill skill = GetEntity(upcomingTurns[0]).EquippedCombatSkills[(int)currentlySelectedSkill];
		GameObject[] proxyArr = GetProxies(targets);
		Entity[] entityArr = GetEntities(targets);

		for(int i = 0; i < targets.Length; i++)
		{
			Instantiate(skill.FxPrefab, proxyArr[i].transform);
			ApplySkill(GetEntity(upcomingTurns[0]), entityArr[i], skill);
		}
		
		// wait until the dmg fx has faded
		while (true)
		{
			if (proxyArr[0].transform.childCount == 0) break;
			yield return null;
		}

		// initiate next turn by ending current one
		EndTurn();
	}

	private void ApplySkill(Entity caster, Entity target, CombatSkill skill)
	{
		int actualDamage = (int)Mathf.Max(0f, caster.currentAttack * skill.DamageMultiplier - target.currentDefense);
		ApplyDamage(target, actualDamage);
	}

	private void ApplyDamage(Entity target, int trueDamage)
	{
		target.currentHealth = Mathf.Max(0, target.currentHealth - trueDamage);
		Debug.Log(target.currentHealth);
	}
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
        if (upcomingTurns[0].x == 1 || combatants[upcomingTurns[0].x, upcomingTurns[0].y].EquippedCombatSkills.Length <= skillID) return;

		currentlySelectedSkill = skillID;
    }
	#endregion
}