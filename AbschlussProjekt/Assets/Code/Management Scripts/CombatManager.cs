﻿using CombatEffectElements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
	#region Variables
	// change this \/ to DataContainer ref
	[SerializeField] private CombatPanel combatPanel;
	[SerializeField] private EventSystem eventSystem;
	[SerializeField] private Color greenColor;
	[SerializeField] private Color redColor;

	// first dimension for team (0 player, 1 opponent), second dimension for specific character
	private Entity[,] combatants;
    private Proxy[,] proxies;
	private bool[,] selectableTargets;
	private List<Vector2Int> upcomingTurns = new List<Vector2Int>();

	private int m_currentlySelectedSkill;
	private int currentlySelectedSkill
	{
		get
		{
			return m_currentlySelectedSkill;
		}
		set
		{
			m_currentlySelectedSkill = value;
			UpdateSelectableTargets();
			UpdateSkillDescriptionText();
		}
	}

	private Vector2Int m_currentlyInspectedEntityPos;
	private Vector2Int currentlyInspectedEntityPos
	{
		get
		{
			return m_currentlyInspectedEntityPos;
		}
		set
		{
			if(m_currentlyInspectedEntityPos != value)
			{
				//GetProxy(value).Wobble();
				m_currentlyInspectedEntityPos = value;
				UpdateEntityInspectionWindow();
			}
			
		}
	}
	
	private LayerMask clickableLayers;
	private float healthbarAdjustmentSpeed = 10f;
	#endregion

	#region Setup
	private void Start()
	{
		//Time.timeScale = 5f;
		clickableLayers = AssetManager.Instance.Settings.LoadAsset<MiscSettings>("Misc Settings").clickableLayers;
	}
	
	public void StartCombat(Entity[] playerTeam, Entity[] opposingTeam)
	{
		combatants = new Entity[2, (playerTeam.Length > opposingTeam.Length) ? playerTeam.Length : opposingTeam.Length];
		proxies = new Proxy[2, combatants.GetLength(1)];
		selectableTargets = new bool[2, combatants.GetLength(1)];

		for (int x = 0; x < combatants.GetLength(0); x++)
			for (int y = 0; y < combatants.GetLength(1); y++)
				combatants[x, y] = (x == 0) ? playerTeam[y] : opposingTeam[y];

		InitializeEntities();
		InstantiateProxyPrefabs();
		StartCoroutine(UpdateHealthBars());

		// initiate combat loop
		LaunchNextTurn();
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
					tempEntity.InitializeSkills();
				}
			}
		}
    }

    private void InstantiateProxyPrefabs()
    {
        for (int x = 0; x < proxies.GetLength(0); x++)
        {
            for (int y = 0; y < proxies.GetLength(1); y++)
            {
				GameObject combatPrefab = combatPanel.CharacterPositions[x, y];
                if(combatants[x, y] == null) continue;
				
				// spawn proxy (proxy => represents an entity in the world)
                GameObject tempProxyGO = Instantiate(combatants[x, y].CharDataContainer.Prefab);
				Proxy tempProxy = tempProxyGO.GetComponent<Proxy>();
				tempProxy.Entity = GetEntity(new Vector2Int(x, y));
				tempProxy.Entity.currentCombatPosition = new Vector2Int(x, y);

				Vector3 proxyPos = AssetManager.Instance.MainCam.ScreenToWorldPoint(combatPrefab.transform.position);
				if (x == 1)
				{
					tempProxyGO.GetComponent<SpriteRenderer>().flipX = true;
					//tempProxyGO.transform.localScale = new Vector3(-1f, 1f, 1f); // flip if its an opponent proxy
					//tempProxy.CombatEffectPool.transform.parent.localScale = 
					//	new Vector3(
					//		tempProxy.CombatEffectPool.transform.parent.localScale.x * -1f, 
					//		tempProxy.CombatEffectPool.transform.parent.localScale.y, 
					//		1f); // and counterflip status canvas
				}
				proxyPos.z = 0f;
                tempProxyGO.transform.position = proxyPos;
                proxies[x, y] = tempProxy;
            }
        }
    }
	#endregion

	#region Combat Loop
	#region User Input
	private void Update()
	{
		CheckForUserInput();
	}

	private void CheckForUserInput()
	{
		RaycastHit2D hit = Physics2D.Raycast(AssetManager.Instance.MainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, clickableLayers);
		if (hit.collider != null)
		{
			Proxy hitProxy = hit.transform.GetComponent<Proxy>();
			if (Input.GetMouseButtonDown(0)) OnCharacterSelect(hitProxy.Entity.currentCombatPosition);
			else currentlyInspectedEntityPos = hitProxy.Entity.currentCombatPosition;
		}
		else currentlyInspectedEntityPos = upcomingTurns[0];

	}

	private void OnCharacterSelect(Vector2Int characterPos)
	{
		if (!GetButtonsEnabled() || GetEntity(characterPos).currentHealth == 0 || !selectableTargets[characterPos.x, characterPos.y]) return;

		SetButtonsEnabled(false);
		UseCombatSkill(characterPos);
	}
	#endregion

	#region Turn Management
	private void LaunchNextTurn()
	{
		if (upcomingTurns.Count == 0) ProgressInits();
		HandleCurrentTurn();
	}

	private void HandleCurrentTurn()
	{
		HandleCombatEffects(true);
		currentlySelectedSkill = 0; // change this to first skill of that character
		ProgressCooldowns();
		UpdateSkillIcons();
		UpdateEntityInspectionWindow();

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
			combatPanel.EventSystem.SetSelectedGameObject(null);
			combatPanel.EventSystem.SetSelectedGameObject(combatPanel.TeamSkillImage[0].gameObject);
		}

		UpdateSelectableTargets();
	}

	private void ProgressCooldowns()
	{
		Entity entity = GetEntity(upcomingTurns[0]);
		List<CombatSkill> skills = new List<CombatSkill>(entity.currentSkillCooldowns.Keys);
		for(int i = 0; i < skills.Count; i++)
		{
			if (entity.currentSkillCooldowns[skills[i]] > 0) entity.currentSkillCooldowns[skills[i]]--;
		}
	}
	
	private void EndTurn()
	{
		HandleCombatEffects(false);
		GetEntity(upcomingTurns[0]).currentInitiative = 0;
		upcomingTurns.RemoveAt(0);

		// start next turn if combat is still ongoing
		if (!CheckForCombatEnd()) LaunchNextTurn();
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
		Entity currentlyActiveEntity = combatants[upcomingTurns[0].x, upcomingTurns[0].y];
		bool playerTurn = upcomingTurns[0].x == 0;

		for (int i = -2; i < combatPanel.TeamSkillImage.Length; i++)
        {
			switch (i)
			{
				case -2:
					combatPanel.PassImage.gameObject.SetActive(playerTurn);
					//if (playerTurn) combatPanel.RepositioningImage.sprite = currentlyActiveEntity.PassSkill.SkillIcon;
					continue;

				case -1:
					combatPanel.RepositioningImage.gameObject.SetActive(playerTurn);
					if (playerTurn) combatPanel.RepositioningImage.sprite = currentlyActiveEntity.EquippedRepositioningSkill.SkillIcon;
					continue;

				default:
					combatPanel.TeamSkillImage[i].gameObject.SetActive(playerTurn);
					break;
			}


			// update icons
			CombatSkill skill = currentlyActiveEntity.EquippedCombatSkills[i];
			combatPanel.TeamSkillImage[i].sprite = (skill == null) 
				? null 
				: skill.SkillIcon;

			// update cooldowns
			TextMeshProUGUI cooldownText = combatPanel.Cooldowns[i];
			if (skill != null)
			{
				int cooldown = currentlyActiveEntity.currentSkillCooldowns[skill];
				cooldownText.SetText(cooldown.ToString());

				bool coolingDown = cooldown != 0;
				cooldownText.enabled = coolingDown;
				combatPanel.TeamSkillImage[i].GetComponent<Button>().enabled = !coolingDown;

				combatPanel.CooldownCoverImages[i].fillAmount = (float)currentlyActiveEntity.currentSkillCooldowns[skill] / (float)skill.Cooldown;

				//Color color = combatPanel.TeamSkillImage[i].color;
				//combatPanel.TeamSkillImage[i].color = new Color(color.r, color.g, color.b, (cooldown != 0) ? .25f : 1f);
			}
			else cooldownText.SetText("");
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

	private IEnumerator UpdateHealthBar(Vector2Int target)
	{
		Entity targetEntity = GetEntity(target);
		Proxy targetProxy = GetProxy(target);
		while (true)
		{
			bool check = targetEntity != null;
			targetProxy.HealthBar.gameObject.SetActive(check);
			if (check)
			{
				targetProxy.HealthBar.maxValue = targetEntity.currentMaxHealth;
				targetProxy.HealthBar.value =
					Mathf.MoveTowards
					(targetProxy.HealthBar.value,
					targetEntity.currentHealth,
					healthbarAdjustmentSpeed * Time.deltaTime);
				if (targetProxy.HealthBar.value == targetEntity.currentHealth) break;
			}
			else break;
			yield return null;
		}
	}
	private IEnumerator UpdateHealthBars()
	{
		bool done = false;
		while (true)
		{
			done = true;
			for (int x = 0; x < proxies.GetLength(0); x++)
			{
				for (int y = 0; y < proxies.GetLength(1); y++)
				{
					bool check = combatants[x, y] != null;
					proxies[x, y].HealthBar.gameObject.SetActive(check);
					if (check)
					{
						proxies[x, y].HealthBar.maxValue = combatants[x, y].currentMaxHealth;
						proxies[x, y].HealthBar.value =
							Mathf.MoveTowards
							(proxies[x, y].HealthBar.value,
							combatants[x, y].currentHealth,
							healthbarAdjustmentSpeed * Time.deltaTime);
						if (proxies[x, y].HealthBar.value != combatants[x, y].currentHealth) done = false;
					}
				}
			}
			if (done) break;
			yield return null;
		}
	}

	private void UpdateSelectableTargets()
	{
		if (currentlySelectedSkill == -1) // move
		{
			for (int x = 0; x < proxies.GetLength(0); x++)
			{
				for (int y = 0; y < proxies.GetLength(1); y++)
				{
					selectableTargets[x, y] = x == upcomingTurns[0].x && (y == upcomingTurns[0].y - 1 || y == upcomingTurns[0].y + 1);
					proxies[x, y].SetIndicatorActive(x, selectableTargets[x, y]);
				}
			}
			return;
		}
		else if (currentlySelectedSkill == -2) // pass
		{
			for (int x = 0; x < proxies.GetLength(0); x++)
			{
				for (int y = 0; y < proxies.GetLength(1); y++)
				{
					selectableTargets[x, y] = upcomingTurns[0].x == x && upcomingTurns[0].y == y;
					proxies[x, y].SetIndicatorActive(x, selectableTargets[x, y]);
				}
			}
			return;
		}

		CombatSkill skill = GetCurrentlySelectedSkill();
		for (int x = 0; x < proxies.GetLength(0); x++)
		{
			for (int y = 0; y < proxies.GetLength(1); y++)
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
						else selectableTargets[x, y] =
								skill.CanHitAllies
								&& (Mathf.Abs(upcomingTurns[0].y - y) <= skill.MaxRange)
								&& (Mathf.Abs(upcomingTurns[0].y - y) >= skill.MinRange);
					}
					//enemy team indicator
					else selectableTargets[x, y] =
							skill.CanHitEnemies
							&& (upcomingTurns[0].y + y + 1 <= skill.MaxRange)
							&& (upcomingTurns[0].y + y + 1 >= skill.MinRange);
				}
				proxies[x, y].SetIndicatorActive(x, selectableTargets[x, y]);
			}
		}
	}
	
	private void UpdateSkillDescriptionText()
	{
		string skillDescriptionText = "";

		// player's turn
		if (upcomingTurns[0].x == 0)
		{
			switch (currentlySelectedSkill)
			{
				case -2:
					skillDescriptionText = GetEntity(upcomingTurns[0]).EquippedPassSkill.SkillDescription;
					break;

				case -1:
					skillDescriptionText = GetEntity(upcomingTurns[0]).EquippedRepositioningSkill.SkillDescription;
					break;

				default:
					skillDescriptionText = GetCurrentlySelectedSkill().SkillDescription;
					break;
			}
		}
		// enemy's turn
		else
		{

		}

		combatPanel.skillDescriptionText.SetText(skillDescriptionText);
	}

	#region Inspection Window
	private void UpdateEntityInspectionWindow()
	{
		Entity entity = GetEntity(currentlyInspectedEntityPos);
		UpdateInspectionWindowStatText(entity);
		UpdateInspectionWindowPortrait(entity);
		UpdateInspectionWindowCombatEffects(entity);
	}

	private void UpdateInspectionWindowStatText(Entity entity)
	{
		combatPanel.statDescriptionText.SetText(
			"HP:	" + GetFormattedStatString(entity.currentHealth, entity.currentMaxHealth) + " / " + GetFormattedStatString(entity.currentMaxHealth, entity.baseHealth) +
			"\nATK:	" + GetFormattedStatString(entity.currentAttack, entity.baseAttack) +
			"\nDEF:	" + GetFormattedStatString(entity.currentDefense, entity.baseDefense) +
			"\nSPD:	" + GetFormattedStatString(entity.currentSpeed, entity.baseSpeed));
	}

	private void UpdateInspectionWindowPortrait(Entity entity)
	{
		combatPanel.EntityInspectionPortrait.sprite = entity.CharDataContainer.Portrait;
	}

	private void UpdateInspectionWindowCombatEffects(Entity entity)
	{
		combatPanel.EntityInspectionEffectPool.CopyCombatEffects(GetCombatEffectPool(currentlyInspectedEntityPos));
	}
	#endregion
	#endregion

	#region Character Action Handling
	private void UseCombatSkill(Vector2Int mainTarget)
	{
		Proxy proxy = GetProxy(upcomingTurns[0]);
		CombatSkill skill = GetCurrentlySelectedSkill();
		Entity attacker = GetEntity(upcomingTurns[0]);
		if (attacker.currentSkillCooldowns.ContainsKey(skill)) attacker.currentSkillCooldowns[skill] = skill.Cooldown;
		else attacker.currentSkillCooldowns.Add(skill, skill.Cooldown);

		List<Vector2Int> targetList = new List<Vector2Int>
		{
			mainTarget
		};

		#region Get characters affected by AoE of attack
		// (surroundingAffectedUnits: x => left units; y => right units)
		Vector2Int target;
		// get left targets
		for (int i = 1; i <= skill.SurroundingAffectedUnits.x; i++)  
		{
			target = new Vector2Int(mainTarget.x, mainTarget.y);

			if (target.y > combatants.GetLength(1)) continue;
			
			if (target.x == 0) target.y += i;
			else if (target.x == 1)
			{
				if (target.y - 1 < 0) Debug.Log("overlap from opponent team to player team"); // when the target overlaps teams
				else target.y -= i;
			}

			if (target.y >= combatants.GetLength(1) || GetEntity(target) == null) continue;
			if (GetEntity(target).currentHealth != 0) targetList.Add(target);
		}
		// get right targets 
		for (int i = 1; i <= skill.SurroundingAffectedUnits.y; i++) 
		{
			target = new Vector2Int(mainTarget.x, mainTarget.y);

			if (target.y > combatants.GetLength(1)) continue;
			
			if (target.x == 0)
			{
				if (target.y - i < 0) Debug.Log("overlap from player to opponent team"); // when the target overlaps teams
				else target.y -= i;
			}
			else if (target.x == 1) target.y += i;

			if (target.y >= combatants.GetLength(1) || GetEntity(target) == null) continue;
			if(GetEntity(target).currentHealth != 0) targetList.Add(target);
		}
		#endregion

		Vector2Int[] targets = targetList.ToArray();

		proxy.GetComponent<Animator>().SetTrigger("Attack");
		proxy.AudioSource.PlayOneShot(skill.castSfx);

		// spawn attack fx on enemies with a certain delay (after triggering atk anim)
		StartCoroutine(LaunchAttack(skill.impactDelay, targets, skill));
	}

	private IEnumerator LaunchAttack(float attackDelay, Vector2Int[] targets, CombatSkill skill)
	{
		yield return new WaitForSeconds(attackDelay);

		Proxy[] proxyArr = GetProxies(targets);
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
		if (currentlySelectedSkill == -2) PassTurn();
		else if (currentlySelectedSkill == -1) SwapPositions(caster, target);

		int actualDamage = 
			(skill.AttackMultiplier > 0) // if the attack multiplier is at zero, then its a buff, not an attack (min dmg circumvented) 
			? Mathf.Max(1, (int) (GetEntity(caster).currentAttack * skill.AttackMultiplier - GetEntity(target).currentDefense))
			: (int)(GetEntity(caster).currentAttack * skill.AttackMultiplier);

		ApplyCombatEffects(caster, target, skill);
		ApplyDamage(target, actualDamage);
	}
	
	private void ApplyDamage(Vector2Int target, int trueDamage)
	{
		// clamp new health between 0 and currentMaxHealth
		GetEntity(target).currentHealth = Mathf.Clamp(GetEntity(target).currentHealth - trueDamage, 0, GetEntity(target).currentMaxHealth);
		StartCoroutine(UpdateHealthBar(target));
	}

	private void SwapPositions(Vector2Int posOne, Vector2Int posTwo)
	{
		Proxy proxyOne = GetProxy(posOne);
		Proxy proxyTwo = GetProxy(posTwo);

		// switch positions in access arrays
		proxies[posOne.x, posOne.y] = proxyTwo;
		proxies[posTwo.x, posTwo.y] = proxyOne;
		combatants[posOne.x, posOne.y] = proxyTwo.Entity;
		combatants[posTwo.x, posTwo.y] = proxyOne.Entity;

		//switch positions in scene
		Vector2 posBuffer = proxyOne.transform.localPosition;
		proxyOne.transform.localPosition = proxyTwo.transform.localPosition;
		proxyTwo.transform.localPosition = posBuffer;

		// switch positions in entity class
		Vector2Int combatPosBuffer = proxyOne.Entity.currentCombatPosition;
		proxyOne.Entity.currentCombatPosition = proxyTwo.Entity.currentCombatPosition;
		proxyTwo.Entity.currentCombatPosition = combatPosBuffer;
	}

	private void PassTurn()
	{

	}

	#region Combat Effects
	private void ApplyCombatEffects(Vector2Int caster, Vector2Int target, CombatSkill skill)
	{
		// applied combat effects
		for (int i = 0; i < skill.AppliedCombatEffects.Length; i++)
		{
			CombatEffect effect = skill.AppliedCombatEffects[i];
			if (!GetCombatEffectPool(target).Contains(effect))
				AddCombatEffectToEntity(target, effect);
			else GetCombatEffectPool(target).activeCombatEffectElements.Find(x => x.CombatEffect == effect)
					.Duration = (target == upcomingTurns[0]) ? effect.Duration + 1 : effect.Duration;
		}

		// self inflicted combat effects
		for (int i = 0; i < skill.SelfInflictedCombatEffects.Length; i++)
		{
			CombatEffect effect = skill.SelfInflictedCombatEffects[i];
			if (!GetCombatEffectPool(target).Contains(effect))
				AddCombatEffectToEntity(target, effect);
			else GetCombatEffectPool(target).activeCombatEffectElements.Find(x => x.CombatEffect == effect)
					.Duration = (target == upcomingTurns[0]) ? effect.Duration + 1 : effect.Duration;
		}
		
		UpdateEntityInspectionWindow();
	}

	private void AddCombatEffectToEntity(Vector2Int target, CombatEffect effect)
	{
		CombatEffectUI combatEffectUI = GetCombatEffectPool(target).AddCombatEffect(effect);
		if (target == upcomingTurns[0]) combatEffectUI.Duration++;
		effect.ApplyCombatEffectModifiers(GetEntity(target));
	}

	private void HandleCombatEffects(bool turnStart)
	{
		Entity entity = GetEntity(upcomingTurns[0]);
		CombatEffectPool pool = GetCombatEffectPool(upcomingTurns[0]);
		List<CombatEffectUI> activeEffects = pool.activeCombatEffectElements;
		Queue<CombatEffectUI> effectsToRemove = new Queue<CombatEffectUI>();

		CombatEffectUI tempCEUI;
		for(int i = 0; i < activeEffects.Count; i++)
		{
			tempCEUI = activeEffects[i];
			if (turnStart) tempCEUI.CombatEffect.ActivateActiveEffect(entity);
			else
			{
				if (tempCEUI.Duration > 1) tempCEUI.Duration--;
				else effectsToRemove.Enqueue(activeEffects[i]);
			}
		}

		while (effectsToRemove.Count > 0)
		{
			tempCEUI = effectsToRemove.Dequeue();
			tempCEUI.CombatEffect.RemoveCombatEffectModifiers(entity);
			pool.RemoveCombatEffect(tempCEUI);
		}
	}
	#endregion

	private void TriggerDeath(Vector2Int dyingCharPos)
	{
		upcomingTurns.Remove(dyingCharPos); // TODO: check if this ever throws an exception
		GetEntity(dyingCharPos).currentInitiative = 0;
		GetProxy(dyingCharPos).GetComponent<Animator>().enabled = false;
		//GameObject proxy = GetProxy(dyingCharPos); 
		//proxy.GetComponent<Animator>().enabled = false; // pause anim
		//proxy.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .25f); // slight transparency

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
		HideUIElements();
		Debug.Log("Player Victory: " + playerWon);
	}

	private void HideUIElements()
	{

	}
	#endregion
	#endregion

	#region Utility Methods
	private CombatSkill GetCurrentlySelectedSkill()
	{
		switch (currentlySelectedSkill)
		{
			case -2: return GetEntity(upcomingTurns[0]).EquippedPassSkill;
			case -1: return GetEntity(upcomingTurns[0]).EquippedRepositioningSkill;
			default: return GetEntity(upcomingTurns[0]).EquippedCombatSkills[currentlySelectedSkill];
		}
	}

	private CombatEffectPool GetCombatEffectPool(Vector2Int proxyPos)
	{
		return proxies[proxyPos.x, proxyPos.y].CombatEffectPool;
	}

	private string GetFormattedStatString(int currentStat, int maxStat)
	{
		return 
			(currentStat != maxStat)
			? ("<color=#" + ColorUtility.ToHtmlStringRGB(
				(currentStat < maxStat)
				? redColor 
				: greenColor) + ">" + currentStat + "</color>")
			: currentStat.ToString();
	}

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

	private Proxy GetProxy(Vector2Int proxyPos)
	{
		return proxies[proxyPos.x, proxyPos.y];
	}
	private Proxy[] GetProxies(Vector2Int[] proxPosArr)
	{
		Proxy[] proxieArr = new Proxy[proxPosArr.Length];
		for (int i = 0; i < proxPosArr.Length; i++)
			proxieArr[i] = proxies[proxPosArr[i].x, proxPosArr[i].y];
		return proxieArr;
	}
	
	private void SetButtonsEnabled(bool enableState)
	{
		combatPanel.RepositioningImage.gameObject.SetActive(enableState);
		combatPanel.PassImage.gameObject.SetActive(enableState);

		for (int i = 0; i < combatPanel.TeamSkillImage.Length; i++)
			combatPanel.TeamSkillImage[i].gameObject.SetActive(
				(GetEntity(upcomingTurns[0]).EquippedCombatSkills[i] == null) 
				? false 
				: enableState);
	}
	private bool GetButtonsEnabled()
	{
		bool active = false;

		for (int i = 0; i < combatPanel.TeamSkillImage.Length; i++)
			if (combatPanel.TeamSkillImage[i].gameObject.activeSelf) active = true;

		return active;
	}
	#endregion

	#region Event System Calls
	public void OnSkillSelect(int skillID)
    {
		currentlySelectedSkill = skillID;
    }
	#endregion
}