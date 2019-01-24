using UnityEngine;

public class MainMenuPanel : MonoBehaviour, IUIPanel
{
	public void OnContinuePress()
	{
		AssetManager instance = AssetManager.Instance;
		instance.Load();
		instance.GetManager<GameManager>().LoadAreaAsync(instance.Savestate.CurrentLocation);
	}

	public void OnNewGamePress()
	{
		AssetManager.Instance.CreateNewSavestate();
	}

	public void OnExitPress()
	{
		AssetManager.Instance.GetManager<GameManager>().ExitGame();
	}
}