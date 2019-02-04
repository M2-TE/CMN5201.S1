using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCityPanel : UIPanel
{
	private void Start()
	{
		ToggleVisibility(true);
	}
	public void OnBarracksSelect()
	{
		ToggleVisibility(false);
		AssetManager.Instance.GetManager<BarracksManager>().TogglePanelVisibility();
	}

	public void OnAcademySelect()
	{

	}

	public void OnExpeditionSelect()
	{
		AssetManager instance = AssetManager.Instance;
		instance.GetManager<GameManager>().LoadAreaAsync(instance.LoadArea(instance.Paths.FirstDungeon));
	}
}
