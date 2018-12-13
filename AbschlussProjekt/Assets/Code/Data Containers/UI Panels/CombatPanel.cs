using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatPanel : UIPanel
{
	public Image[] TeamSkillButtons;

	private EventSystem eventSystem;
	public EventSystem EventSystem
	{
		get
		{
			return eventSystem ?? (eventSystem = transform.parent.GetComponentInChildren<EventSystem>());
		}
	}

	private Image[,] selectableIndicators;
	public Image[,] SelectableIndicators
	{
		get
		{
			if (selectableIndicators != null) return selectableIndicators;

			selectableIndicators = new Image[CharacterPositions.GetLength(0), CharacterPositions.GetLength(1)];
			for(int x = 0; x < selectableIndicators.GetLength(0); x++)
				for (int y = 0; y < selectableIndicators.GetLength(1); y++)
					selectableIndicators[x, y] = CharacterPositions[x, y].transform.Find("Target Indicator").GetComponent<Image>();
			return selectableIndicators;
		}
	}

	[SerializeField] private Image[] playerTeamPortraits, opposingTeamPortraits;
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


    [SerializeField] private GameObject[] playerCharacterPositions, opposingTeamCharacterPositions;
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

	[SerializeField] private Slider[] playerHealthBars, opposingTeamHealthBars;
	private Slider[,] healthBars;
	public Slider[,] HealthBars
	{
		get
		{
			if (healthBars != null) return healthBars;

			healthBars = new Slider[2,
				(playerHealthBars.Length > opposingTeamHealthBars.Length)
				? playerHealthBars.Length
				: opposingTeamHealthBars.Length];
			for (int x = 0; x < healthBars.GetLength(0); x++)
				for (int y = 0; y < healthBars.GetLength(1); y++)
					healthBars[x, y] = (x == 0) ? playerHealthBars[y] : opposingTeamHealthBars[y];
			return healthBars;
		}
	}
}