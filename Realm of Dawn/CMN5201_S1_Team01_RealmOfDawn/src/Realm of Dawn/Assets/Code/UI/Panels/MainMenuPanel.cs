using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : UIPanel
{
	[SerializeField] private GameObject creditsOverlay;
	public void OnContinuePress()
	{
		AssetManager instance = AssetManager.Instance;
		instance.GetManager<GameManager>().LoadAreaAsync(instance.Savestate.CurrentLocation);
	}

	public void OnNewGamePress()
	{
		AssetManager.Instance.CreateNewSavestate();
		OnContinuePress();
	}

	public void OnOptionsPress()
	{
		AssetManager.Instance.GetManager<OptionsManager>().ToggleVisibility();
	}

	public void OnExitPress()
	{
		AssetManager.Instance.GetManager<GameManager>().ExitGame();
	}

	public void OnCreditsPress()
	{
		creditsOverlay.SetActive(true);
	}

	public void OnCreditsBackPress()
	{
		creditsOverlay.SetActive(false);
	}
}