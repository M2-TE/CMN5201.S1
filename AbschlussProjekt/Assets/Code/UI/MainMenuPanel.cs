using UnityEngine;

public class MainMenuPanel : MonoBehaviour, IUIPanel
{
	private MainMenuManager mainMenuManager;
	public void Register<T>(T manager) where T : UiManager
	{
		mainMenuManager = manager as MainMenuManager;
	}

	public void OnButtonPress(string buttonType)
	{
		switch (buttonType)
		{
			case "Continue":
				mainMenuManager.OnContinuePress();
				break;

			case "New Game":
				mainMenuManager.OnNewGamePress();
				break;

			case "Exit":
				mainMenuManager.OnExitPress();
				break;

			default: throw new System.Exception("Illegal Button Command");
		}
	}
}