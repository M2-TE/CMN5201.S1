using UnityEngine;

public class MainMenuManager : UiManager, IManager
{
	private MainMenuPanel mainMenuPanel;
	private GameManager gameManager;

	public MainMenuManager()
	{
		AssetManager instance = AssetManager.Instance;
		gameManager = instance.GameManager;

		mainMenuPanel = Object.Instantiate
			 (instance.UIPrefabs.LoadAsset<UIPrefabs>("UIPrefabs").MainMenuUIPrefab, instance.MainCanvas.transform)
			 .GetComponent<MainMenuPanel>();
		mainMenuPanel.Register(this);
	}

	public override void Destroy()
	{
		Object.Destroy(mainMenuPanel.gameObject);
	}

	public override void SetActive(bool activeState)
	{
		mainMenuPanel.gameObject.SetActive(activeState);
	}

	#region Button Calls
	public void OnContinuePress()
	{
		gameManager.LoadGame();
	}

	public void OnNewGamePress()
	{
		gameManager.CreateNewSavestate();
	}

	public void OnExitPress()
	{
		gameManager.ExitGame();
	}
	#endregion
}
