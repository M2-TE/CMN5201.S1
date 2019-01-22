using CombatEffectElements;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatPanel : MonoBehaviour, IUIPanel
{
	[Header("Skills")]
	public Image PassImage;
	public Image RepositioningImage;
	public Image[] TeamSkillImage;
	public Image[] CooldownCoverImages;
	public TextMeshProUGUI[] Cooldowns;

	[Header("Entity Inspection Window")]
	public TextMeshProUGUI skillDescriptionText;
	public TextMeshProUGUI statDescriptionText;

	public Image EntityInspectionPortrait;
	public CombatEffectPool EntityInspectionEffectPool;

	private EventSystem eventSystem;
	public EventSystem EventSystem
	{
		get
		{
			return eventSystem ?? (eventSystem = transform.parent.GetComponentInChildren<EventSystem>());
		}
	}

	[Header("Team Portraits")]
	[SerializeField] private Image[] playerTeamPortraits;
	[SerializeField] private Image[] opposingTeamPortraits;
	private Image[,] teamPortraits;
	public Image[,] TeamPortraits
	{
		get
		{
			if (teamPortraits != null) return teamPortraits;

			teamPortraits = new Image[2,
				(playerTeamPortraits.Length > opposingTeamPortraits.Length)
				? playerTeamPortraits.Length
				: opposingTeamPortraits.Length];
			for (int x = 0; x < teamPortraits.GetLength(0); x++)
				for (int y = 0; y < teamPortraits.GetLength(1); y++)
					teamPortraits[x, y] = (x == 0) ? playerTeamPortraits[y] : opposingTeamPortraits[y];

			return teamPortraits;
		}
	}

	[Header("Character Positions")]
	[SerializeField] private GameObject[] playerCharacterPositions;
	[SerializeField] private GameObject[] opposingTeamCharacterPositions;
	private GameObject[,] characterPositions;
	public GameObject[,] CharacterPositions
	{
		get
		{
			if (characterPositions != null) return characterPositions;

			characterPositions = new GameObject[2,
				(playerCharacterPositions.Length > opposingTeamCharacterPositions.Length)
				? playerCharacterPositions.Length
				: opposingTeamCharacterPositions.Length];
			for (int x = 0; x < characterPositions.GetLength(0); x++)
				for (int y = 0; y < characterPositions.GetLength(1); y++)
					characterPositions[x, y] = (x == 0) ? playerCharacterPositions[y] : opposingTeamCharacterPositions[y];
			return characterPositions;
		}
	}

	private bool m_combatActive;
	public bool combatActive
	{
		get { return m_combatActive; }
		set { m_combatActive = value; gameObject.SetActive(value); }
	}

	public Camera mainCam;

	private CombatManager combatManager;

	private void Awake()
	{
		combatManager = new CombatManager(this);
	}

	private void Update()
	{
		if(combatActive) combatManager.UpdateCombatManager();
	}

	public void StartCombat(Entity[] playerTeam, Entity[] opposingTeam)
	{
		combatActive = true;
		combatManager.StartCombat(playerTeam, opposingTeam);
	}

	public void OnSkillSelect(int skillID)
	{
		if (combatActive) combatManager.OnSkillSelect(skillID);
	}
}