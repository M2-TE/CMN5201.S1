using UnityEngine;

public class MainMenuManager : IManager
{
	private MainMenuPanel mainMenuPanel;

	public MainMenuManager()
	{
		mainMenuPanel = AssetManager.Instance.MainMenuUI.GetComponent<MainMenuPanel>();
		mainMenuPanel.Register(this);
	}

	#region Button Calls
	public void OnContinuePress()
	{
		Debug.Log("Continue");
	}

	public void OnNewGamePress()
	{
		// savefile = new Savestate();
	}

	public void OnExitPress()
	{
		AssetManager.Instance.GameManager.ExitGame();
	}
	#endregion
}
