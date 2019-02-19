using CombatEffectElements;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatManager : Manager
{
	#region Variables
	// first dimension for team (0 player, 1 opponent), second dimension for specific character
	private Entity[,] entities;
	private Proxy[,] proxies;
	private bool[,] selectableTargets;
	private List<Vector2Int> upcomingTurns = new List<Vector2Int>();
	private StringBuilder stringBuilder = new StringBuilder();

	private int m_currentlySelectedSkill;
	private int currentlySelectedSkill
	{
		get { return m_currentlySelectedSkill; }
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
		get { return m_currentlyInspectedEntityPos; }
		set
		{
			if (m_currentlyInspectedEntityPos != value)
			{
				m_currentlyInspectedEntityPos = value;
				UpdateEntityInspectionWindow();
			}
		}
	}

	private EventSystem eventSystem;
	private CombatPanel combatPanel;
	private CombatManagerSettings settings;
	private LayerMask clickableLayers;
	#endregion

	#region Startup
	public CombatManager()
	{
		settings = AssetManager.Instance.LoadBundle<CombatManagerSettings>(AssetManager.Instance.Paths.SettingsPath, "Combat Manager Settings");
		clickableLayers = settings.clickableLayers;
	}

	public void RegisterCombatPanel(CombatPanel combatPanel)
	{
		this.combatPanel = combatPanel;
		eventSystem = combatPanel.GetComponentInChildren<EventSystem>();
	}

	public void StartCombat(Entity[] playerTeam, Entity[] opposingTeam)
	{
		entities = new Entity[2, (playerTeam.Length > opposingTeam.Length) ? playerTeam.Length : opposingTeam.Length];
		proxies = new Proxy[2, entities.GetLength(1)];
		selectableTargets = new bool[2, entities.GetLength(1)];
		
		for (int x = 0; x < entities.GetLength(0); x++)
			for (int y = 0; y < entities.GetLength(1); y++)
				entities[x, y] = (x == 0)
					? playerTeam.Length > y ? playerTeam[y] : null
					: opposingTeam.Length > y ? opposingTeam[y] : null;

		combatPanel.CombatActive = true;
		InitializeEntities();
		InstantiateProxyPrefabs();
		combatPanel.StartCoroutine(UpdateHealthBars(true));

		// initiate combat loop
		LaunchNextTurn();
	}

	private void InitializeEntities()
	{
		Entity tempEntity;
		for (int x = 0; x < entities.GetLength(0); x++)
		{
			for (int y = 0; y < entities.GetLength(1); y++)
			{
				tempEntity = entities[x, y];
				if (tempEntity != null)
				{
					tempEntity.CurrentInitiative = 0;
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
				if (entities[x, y] == null) continue;

				// spawn proxy (proxy => represents an entity in the world)
				GameObject tempProxyGO = Object.Instantiate(entities[x, y].CharDataContainer.Prefab);
				Proxy tempProxy = tempProxyGO.GetComponent<Proxy>();
				tempProxy.Entity = GetEntity(new Vector2Int(x, y));
				tempProxy.Entity.currentCombatPosition = new Vector2Int(x, y);

				Vector3 proxyPos = combatPanel.mainCam.ScreenToWorldPoint(combatPrefab.transform.position);
				if (x == 1)
				{
					//tempProxyGO.GetComponent<SpriteRenderer>().flipX = true;
					tempProxyGO.transform.localScale = new Vector3(-1f, 1f, 1f); // flip if its an opponent proxy
					tempProxy.CombatEffectPool.transform.parent.localScale =
						new Vector3(
							tempProxy.CombatEffectPool.transform.parent.localScale.x * -1f,
							tempProxy.CombatEffectPool.transform.parent.localScale.y,
							1f); // and counterflip status canvas
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

	public void UpdateCombatManager()
	{
		RaycastHit2D hit = Physics2D.Raycast(combatPanel.mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, clickableLayers);
		if (hit.collider != null)
		{
			Proxy hitProxy = hit.transform.GetComponent<Proxy>();
			if (Input.GetMouseButtonDown(0)) OnCharacterSelect(hitProxy.Entity.currentCombatPosition);
			else currentlyInspectedEntityPos = hitProxy.Entity.currentCombatPosition;
		}
		else if(combatPanel.CombatActive) currentlyInspectedEntityPos = upcomingTurns[0];

	}

	private void OnCharacterSelect(Vector2Int characterPos)
	{
		if (!GetButtonsEnabled() || GetEntity(characterPos).CurrentHealth == 0 || !selectableTargets[characterPos.x, characterPos.y]) return;

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

		GetProxy(upcomingTurns[0]).SetSelected(true);
	}

	private void ProgressCooldowns()
	{
		Entity entity = GetEntity(upcomingTurns[0]);
		List<CombatSkill> skills = new List<CombatSkill>(entity.currentSkillCooldowns.Keys);
		for (int i = 0; i < skills.Count; i++)
		{
			if (entity.currentSkillCooldowns[skills[i]] > 0) entity.currentSkillCooldowns[skills[i]]--;
		}
	}

	private void EndTurn()
	{
		HandleCombatEffects(false);
		GetProxy(upcomingTurns[0]).SetSelected(false);
		GetEntity(upcomingTurns[0]).CurrentInitiative = 0;
		upcomingTurns.RemoveAt(0);

		// start next turn if combat is still ongoing
		if (!CheckForCombatEnd()) LaunchNextTurn();
	}

	private void ControlOpponentTurn()
	{
		Entity opponentEntity = GetEntity(upcomingTurns[0]);

		// select AI's skill
		List<int> selectableSkills = new List<int>();
		int counter = 0;
		foreach (int cooldown in opponentEntity.currentSkillCooldowns.Values)
		{
			if (cooldown == 0) selectableSkills.Add(counter);
			counter++;
		}

		while (selectableSkills.Count > 0)
		{
			currentlySelectedSkill = selectableSkills[Random.Range(0, selectableSkills.Count)];

			//currentlySelectedSkill = 0;

			List<Vector2Int> validTargets = new List<Vector2Int>();
			for (int x = 0; x < selectableTargets.GetLength(0); x++)
				for (int y = 0; y < selectableTargets.GetLength(1); y++)
					if (selectableTargets[x, y] == true) validTargets.Add(new Vector2Int(x, y));
			if (validTargets.Count == 0)
			{
				if (selectableSkills.Count == 0)
				{
					EndTurn(); // skip turn if none of the available abilites can hit a target
					break;
				}
				else
				{
					selectableSkills.Remove(currentlySelectedSkill);
					continue;
				}
			}
			else
			{
				UseCombatSkill(validTargets[Random.Range(0, validTargets.Count)]);
				break;
			}
		}
	}

	private void ProgressInits()
	{
		for (int x = 0; x < entities.GetLength(0); x++)
		{
			for (int y = 0; y < entities.GetLength(1); y++)
			{
				Entity tempEntity = entities[x, y];
				if (tempEntity == null || tempEntity.CurrentHealth == 0) continue;

				tempEntity.CurrentInitiative += tempEntity.CurrentSpeed;
				if (tempEntity.CurrentInitiative >= 100)
					upcomingTurns.Add(new Vector2Int(x, y));
			}
		}
		// Repeat until an entity gains a turn
		if (upcomingTurns.Count == 0) ProgressInits();

		// Sort all upcoming turns by total initiative (entities with higher init come first)
		else upcomingTurns.Sort((first, second)
				=> GetEntity(second).CurrentInitiative.CompareTo(GetEntity(first).CurrentInitiative));
	}
	#endregion

	#region UI Updates
	private void UpdateSkillIcons()
	{
		Entity currentlyActiveEntity = entities[upcomingTurns[0].x, upcomingTurns[0].y];
		bool playerTurn = upcomingTurns[0].x == 0;

		for (int i = -2; i < combatPanel.TeamSkillImage.Length; i++)
		{
			switch (i)
			{
				case -2:
					combatPanel.PassImage.gameObject.SetActive(playerTurn);
					if (playerTurn) combatPanel.PassImage.sprite = currentlyActiveEntity.EquippedPassSkill.SkillIcon;
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
		for (int x = 0; x < entities.GetLength(0); x++)
		{
			for (int y = 0; y < entities.GetLength(1); y++)
			{
				bool check = entities[x, y] != null;
				combatPanel.TeamPortraits[x, y].enabled = check;
				combatPanel.TeamPortraits[x, y].sprite = check ? entities[x, y].CharDataContainer.Portrait : null;
			}
		}
	}

	private IEnumerator UpdateHealthBar(Vector2Int target, bool setInstantly = false)
	{
		Entity targetEntity = GetEntity(target);
		Proxy targetProxy = GetProxy(target);
		while (true)
		{
			bool check = targetEntity != null;
			targetProxy.HealthBar.gameObject.SetActive(check);
			if (check)
			{
				targetProxy.HealthBar.maxValue = targetEntity.CurrentMaxHealth;
				targetProxy.HealthBar.value =
					Mathf.MoveTowards
					(targetProxy.HealthBar.value,
					targetEntity.CurrentHealth,
					settings.healthbarAdjustmentSpeed * Time.deltaTime);
				if (targetProxy.HealthBar.value == targetEntity.CurrentHealth) break;
			}
			else break;
			if(!setInstantly) yield return null;
		}
	}
	private IEnumerator UpdateHealthBars(bool setInstantly = false)
	{
		bool done = false;
		while (true)
		{
			done = true;
			for (int x = 0; x < proxies.GetLength(0); x++)
			{
				for (int y = 0; y < proxies.GetLength(1); y++)
				{
					bool check = entities[x, y] != null;
					if (check)
					{
						//proxies[x, y].HealthBar.gameObject.SetActive(check);
						proxies[x, y].HealthBar.maxValue = entities[x, y].CurrentMaxHealth;
						proxies[x, y].HealthBar.value =
							Mathf.MoveTowards
							(proxies[x, y].HealthBar.value,
							entities[x, y].CurrentHealth,
							settings.healthbarAdjustmentSpeed * Time.deltaTime);
						if (proxies[x, y].HealthBar.value != entities[x, y].CurrentHealth) done = false;
					}
				}
			}
			if (done) break;
			if(!setInstantly) yield return null;
		}
	}

	private void UpdateSelectableTargets()
	{
		CombatSkill skill = GetCurrentlySelectedSkill();
		if (skill == null) return;

		for (int x = 0; x < proxies.GetLength(0); x++)
		{
			for (int y = 0; y < proxies.GetLength(1); y++)
			{
				// dont show indicator if: 1: Its an enemy's turn. 2: The spot is empty. 3: The character is dead (and no revive skill is in use)
				Vector2Int indicPos = new Vector2Int(x, y);
				if (GetEntity(indicPos) == null
					|| GetEntity(indicPos).CurrentHealth == 0) selectableTargets[x, y] = false;
				else
				{
					if (x == 0) // current iterated entity is an ally
					{
						if(upcomingTurns[0].x == 0) // ally turn
						{
							if (y == upcomingTurns[0].y) selectableTargets[x, y] = skill.CanHitSelf;
							else selectableTargets[x, y] =
									skill.CanHitAllies
									&& (Mathf.Abs(upcomingTurns[0].y - y) <= skill.MaxRange)
									&& (Mathf.Abs(upcomingTurns[0].y - y) >= skill.MinRange);
						}
						else // enemy turn
						{
							selectableTargets[x, y] =
								skill.CanHitEnemies
								&& (upcomingTurns[0].y + y + 1 <= skill.MaxRange)
								&& (upcomingTurns[0].y + y + 1 >= skill.MinRange);
						}
					}
					else // current iterated entity is an enemy
					{
						if(upcomingTurns[0].x == 0) // ally turn
						{
							selectableTargets[x, y] =
								skill.CanHitEnemies
								&& (upcomingTurns[0].y + y + 1 <= skill.MaxRange)
								&& (upcomingTurns[0].y + y + 1 >= skill.MinRange);
						}
						else // enemy turn
						{
							if (y == upcomingTurns[0].y) selectableTargets[x, y] = skill.CanHitSelf;
							else selectableTargets[x, y] =
									skill.CanHitAllies
									&& (Mathf.Abs(upcomingTurns[0].y - y) <= skill.MaxRange)
									&& (Mathf.Abs(upcomingTurns[0].y - y) >= skill.MinRange);
						}
					}
				}
				if (proxies[x, y] != null)
				{
					if (upcomingTurns[0].x == 0) proxies[x, y].SetIndicatorActive(x, selectableTargets[x, y]);
					else proxies[x, y].SetIndicatorActive(x, false);
				}
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

		combatPanel.SkillDescriptionText.SetText(skillDescriptionText);
	}

	#region Inspection Window
	private void UpdateEntityInspectionWindow()
	{
		Entity entity = GetEntity(currentlyInspectedEntityPos);
		UpdateInspectionWindowStatText(entity);
		combatPanel.EntityInspectionPortrait.sprite = entity.CharDataContainer.Portrait;
		combatPanel.EntityInspectionEffectPool.CopyCombatEffects(GetCombatEffectPool(currentlyInspectedEntityPos));
		combatPanel.EntityNameText.text = entity.Name;
		combatPanel.EntityLevelText.text = "Level " + entity.CurrentLevel;
	}

	private void UpdateInspectionWindowStatText(Entity entity)
	{
		stringBuilder.Clear();
		stringBuilder.Append("HP: ");
		stringBuilder.Append(GetFormattedStatString(entity.CurrentHealth, entity.CurrentMaxHealth));
		stringBuilder.Append(" / ");
		stringBuilder.AppendLine(GetFormattedStatString(entity.CurrentMaxHealth, entity.BaseHealth));

		stringBuilder.Append("ATK: ");
		stringBuilder.AppendLine(GetFormattedStatString(entity.CurrentAttack, entity.BaseAttack));

		stringBuilder.Append("DEF: ");
		stringBuilder.AppendLine(GetFormattedStatString(entity.CurrentDefense, entity.BaseDefense));

		stringBuilder.Append("SPD: ");
		stringBuilder.Append(GetFormattedStatString(entity.CurrentSpeed, entity.BaseSpeed));

		combatPanel.StatDescriptionText.SetText(stringBuilder.ToString());

		//combatPanel.StatDescriptionText.SetText(
		//	"HP:	" + GetFormattedStatString(entity.CurrentHealth, entity.CurrentMaxHealth) + " / " + GetFormattedStatString(entity.CurrentMaxHealth, entity.BaseHealth) +
		//	"\nATK:	" + GetFormattedStatString(entity.CurrentAttack, entity.BaseAttack) +
		//	"\nDEF:	" + GetFormattedStatString(entity.CurrentDefense, entity.BaseDefense) +
		//	"\nSPD:	" + GetFormattedStatString(entity.CurrentSpeed, entity.BaseSpeed));
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
		// surroundingAffectedUnits: x => left units; y => right units
		Vector2Int aoeRange = upcomingTurns[0].x == 0  // swap x and y of aoe on the enemys attacks
			? skill.SurroundingAffectedUnits 
			: new Vector2Int(skill.SurroundingAffectedUnits.y, skill.SurroundingAffectedUnits.x);

		Vector2Int target;
		// get left targets
		for (int i = 1; i <= aoeRange.x; i++)
		{
			target = new Vector2Int(mainTarget.x, mainTarget.y);

			if (target.y > entities.GetLength(1)) continue;

			if (target.x == 0) target.y += i;
			else if (target.x == 1)
			{
				if (target.y - i < 0) continue; // when the target overlaps teams
				else target.y -= i;
			}

			if (target.y >= entities.GetLength(1) || GetEntity(target) == null) continue;
			if (GetEntity(target).CurrentHealth != 0) targetList.Add(target);
		}
		// get right targets 
		for (int i = 1; i <= aoeRange.y; i++)
		{
			target = new Vector2Int(mainTarget.x, mainTarget.y);

			if (target.y > entities.GetLength(1)) continue;

			if (target.x == 0)
			{
				if (target.y - i < 0) continue; // when the target overlaps teams
				else target.y -= i;
			}
			else if (target.x == 1) target.y += i;

			if (target.y >= entities.GetLength(1) || GetEntity(target) == null) continue;
			if (GetEntity(target).CurrentHealth != 0) targetList.Add(target);
		}
		#endregion

		Vector2Int[] targets = targetList.ToArray();

		if (skill.AnimatorTrigger != "")
			proxy.GetComponent<Animator>().SetTrigger(skill.AnimatorTrigger);
		//proxy.PlaySfx(skill.castSfx);
		combatPanel.StartCoroutine(PlayCastSfx(proxy, skill.castSfx, skill.castAudioDelay));

		// spawn attack fx on enemies with a certain delay (after triggering atk anim)
		combatPanel.StartCoroutine(LaunchAttack(targets, skill));
	}

	private IEnumerator PlayCastSfx(Proxy proxy, AudioClip[] castSfx, float delay)
	{
		yield return new WaitForSeconds(delay);
		proxy.PlaySfx(castSfx);
	}

	private IEnumerator LaunchAttack(Vector2Int[] targets, CombatSkill skill)
	{
		Proxy[] proxyArr = GetProxies(targets);
		SoloEffect tempEffect = null;

		yield return new WaitForSeconds(skill.impactSpawnDelay);

		for (int i = 0; i < targets.Length; i++)
		{
			tempEffect = Object.Instantiate(skill.FxPrefab, proxyArr[i].transform).GetComponent<SoloEffect>();
			combatPanel.StartCoroutine(tempEffect.PlaySfx(skill.impactSfx, skill.impactAudioDelay));
			combatPanel.StartCoroutine(tempEffect.PlayAnimation(skill.impactAnimationDelay));
		}

		// apply skill to targets with timing mod
		combatPanel.StartCoroutine(PrepareCombatSkillApplication(upcomingTurns[0], targets, skill, skill.impactDmgDelay));

		// wait until the fx has faded
		yield return new WaitForSeconds(tempEffect.RawCombatDuration + skill.impactAnimationDelay);

		// check for targets death
		for (int i = 0; i < targets.Length; i++)
			if (GetEntity(targets[i]).CurrentHealth == 0) TriggerDeath(GetEntity(targets[i]).currentCombatPosition);

		EndTurn();
	}

	private IEnumerator PrepareCombatSkillApplication(Vector2Int caster, Vector2Int[] targets, CombatSkill skill, float dmgDelay)
	{
		yield return new WaitForSeconds(dmgDelay);

		for (int i = 0; i < targets.Length; i++)
			ApplyCombatSkill(caster, targets[i], skill);
	}

	private void ApplyCombatSkill(Vector2Int caster, Vector2Int target, CombatSkill skill)
	{
		if (currentlySelectedSkill == -2) PassTurn();
		else if (currentlySelectedSkill == -1) SwapPositions(caster, target);

		int actualDamage =
			(skill.AttackMultiplier > 0) // if the attack multiplier is at zero, then its a buff, not an attack (min dmg circumvented) 
			? Mathf.Max(1, (int)(GetEntity(caster).CurrentAttack * skill.AttackMultiplier - GetEntity(target).CurrentDefense))
			: (int)(GetEntity(caster).CurrentAttack * skill.AttackMultiplier);

		ApplyCombatEffects(caster, target, skill);
		ApplyDamage(target, actualDamage);
	}

	private void ApplyDamage(Vector2Int target, int trueDamage)
	{
		// clamp new health between 0 and currentMaxHealth
		GetEntity(target).CurrentHealth = Mathf.Clamp(GetEntity(target).CurrentHealth - trueDamage, 0, GetEntity(target).CurrentMaxHealth);
		combatPanel.StartCoroutine(UpdateHealthBar(target));
	}

	private void SwapPositions(Vector2Int posOne, Vector2Int posTwo)
	{
		Proxy proxyOne = GetProxy(posOne);
		Proxy proxyTwo = GetProxy(posTwo);

		// switch positions in access arrays
		proxies[posOne.x, posOne.y] = proxyTwo;
		proxies[posTwo.x, posTwo.y] = proxyOne;
		entities[posOne.x, posOne.y] = proxyTwo.Entity;
		entities[posTwo.x, posTwo.y] = proxyOne.Entity;

		//switch positions in scene
		Vector2 posBuffer = proxyOne.transform.localPosition;
		proxyOne.transform.localPosition = proxyTwo.transform.localPosition;
		proxyTwo.transform.localPosition = posBuffer;

		// switch positions in entity class
		Vector2Int combatPosBuffer = proxyOne.Entity.currentCombatPosition;
		proxyOne.Entity.currentCombatPosition = proxyTwo.Entity.currentCombatPosition;
		proxyTwo.Entity.currentCombatPosition = combatPosBuffer;

		// recalculate upcoming turns
		upcomingTurns.Clear();
		ProgressInits();
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
			if (!GetCombatEffectPool(caster).Contains(effect))
				AddCombatEffectToEntity(caster, effect);
			else GetCombatEffectPool(caster).activeCombatEffectElements.Find(x => x.CombatEffect == effect)
					.Duration = (caster == upcomingTurns[0]) ? effect.Duration + 1 : effect.Duration;
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
		for (int i = 0; i < activeEffects.Count; i++)
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
		GetEntity(dyingCharPos).CurrentInitiative = 0;
		GetProxy(dyingCharPos).GetComponent<Animator>().enabled = false;
		//GetProxy(dyingCharPos).gameObject.SetActive(false);
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

		for (int x = 0; x < entities.GetLength(0); x++)
		{
			for (int y = 0; y < entities.GetLength(1); y++)
			{
				if (entities[x, y] != null && entities[x, y].CurrentHealth != 0)
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
		combatPanel.CombatActive = false;

		//Debug.Log("Player Victory: " + playerWon);
		if (playerWon == true)
		{
			AwardExp();
			AssetManager.Instance.GetManager<ChestManager>().ChestPanel.Open(OnChestClose);
		}
		else
		{
			AssetManager.Instance.GetManager<DungeonManager>().HealEntireParty(1f);
			AssetManager.Instance.LoadArea(AssetManager.Instance.Paths.DefaultLocation);
		}
	}

	private void OnChestClose()
	{
		AssetManager.Instance.GetManager<DungeonManager>().HandleCombatVictory();
	}

	private void AwardExp()
	{
		Entity tempEntity;
		float totalExpAward = 0f;
		for (int i = 0; i < entities.GetLength(1); i++)
		{
			tempEntity = entities[1, i];
			if (tempEntity == null) continue;
			totalExpAward 
				+= tempEntity.CharDataContainer.baseExpYield 
				* tempEntity.CharDataContainer.expYieldGrowth 
				* tempEntity.CurrentLevel;
		}

		int allyCount = 0;
		for (int i = 0; i < entities.GetLength(1); i++)
			if (entities[0, i] != null) allyCount++;

		for (int i = 0; i < entities.GetLength(1); i++)
		{
			tempEntity = entities[0, i];
			if (tempEntity == null) continue;
			tempEntity.AddExp((int)(totalExpAward / allyCount));
		}
	}

	//private void UnloadCombatLevel()
	//{
	//	for (int x = 0; x < proxies.GetLength(0); x++)
	//		for (int y = 0; y < proxies.GetLength(1); y++)
	//			if (proxies[x, y] != null) Object.Destroy(proxies[x, y].gameObject);

	//	AssetManager.Instance.GetManager<GameManager>().UnloadCombatAreaAsync();
	//}
	#endregion
	#endregion

	#region Utility Methods
	private CombatSkill GetCurrentlySelectedSkill()
	{
		switch (currentlySelectedSkill)
		{
			case -2: return GetEntity(upcomingTurns[0]).EquippedPassSkill;
			case -1: return GetEntity(upcomingTurns[0]).EquippedRepositioningSkill;

			case 0: case 1: case 2: case 3:
				return GetEntity(upcomingTurns[0]).EquippedCombatSkills[currentlySelectedSkill];

			default: return null;
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
				? settings.lowerStatColor
				: settings.higherStatColor) + ">" + currentStat + "</color>")
			: currentStat.ToString();
	}

	private Entity GetEntity(Vector2Int entityPos)
	{
		return entities[entityPos.x, entityPos.y];
	}
	private Entity[] GetEntities(Vector2Int[] entityPosArr)
	{
		Entity[] entityArr = new Entity[entityPosArr.Length];
		for (int i = 0; i < entityPosArr.Length; i++)
			entityArr[i] = entities[entityPosArr[i].x, entityPosArr[i].y];
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

	#region Misc
	public void OnSkillSelect(int skillID)
	{
		currentlySelectedSkill = skillID;
	}
	#endregion
}