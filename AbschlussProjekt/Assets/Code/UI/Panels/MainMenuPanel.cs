using UnityEngine;

public class MainMenuPanel : UIPanel
{

	protected override void Awake()
	{
		base.Awake();
		ToggleVisibility(true);
	}
	public void OnContinuePress()
	{
		AssetManager instance = AssetManager.Instance;
		instance.Load();
		instance.GetManager<GameManager>().LoadAreaAsync(instance.Savestate.CurrentLocation);
	}

	public void OnNewGamePress()
	{
		AssetManager.Instance.CreateNewSavestate();
		OnContinuePress();
	}

	public void OnExitPress()
	{
		AssetManager.Instance.GetManager<GameManager>().ExitGame();
	}
}