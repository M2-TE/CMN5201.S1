using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;

public class PauseMenuPanel : UIPanel
{
	private void Start()
	{
		ToggleVisibility(false);

		InputManager inputManager = AssetManager.Instance.GetManager<InputManager>();
		void callback(InputAction.CallbackContext ctx) => HandleEscPress();
		inputManager.AddListener(inputManager.Input.UI.Back, callback);
	}

	private void OnDestroy()
	{
		AssetManager.Instance.GetManager<InputManager>().RemoveListeners(this);
	}

	public void HandleEscPress()
	{
		ToggleVisibility();
	}

	public override void ToggleVisibility(bool visibleState)
	{
		Time.timeScale = (visibleState) ? 0f : 1f;
		base.ToggleVisibility(visibleState);
	}

	#region Buttons
	public void OnContinuePress()
	{
		HandleEscPress();
	}

	public void OnSettingsPress()
	{

	}

	public void OnMainMenuPress()
	{
		AssetManager.Instance.GetManager<GameManager>().LoadAreaAsync
			(AssetManager.Instance.LoadArea(AssetManager.Instance.Paths.MainMenu));
	}

	public void OnExitPress()
	{
		AssetManager.Instance.GetManager<GameManager>().ExitGame();
	}
	#endregion
}
