using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCityPanel : UIPanel
{
	public void OnBarracksSelect()
	{
		AssetManager.Instance.GetManager<BarracksManager>().TogglePanelVisibility();
	}

	public void OnAcademySelect()
	{

	}

	public void OnExpeditionSelect()
	{

	}
}
