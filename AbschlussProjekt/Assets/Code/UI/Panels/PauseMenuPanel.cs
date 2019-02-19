using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;

public class PauseMenuPanel : UIPanel
{
	private InputManager inputManager;
	private bool inputManagerRegistrationBuffered;
	private void Start()
	{
		if (inputManagerRegistrationBuffered)
		{
			inputManager = AssetManager.Instance.GetManager<InputManager>();
			RegisterCallbacks();
		}
	}

	private void OnEnable()
	{
		inputManager = AssetManager.Instance.GetManager<InputManager>();
		if (inputManager == null)
			inputManagerRegistrationBuffered = true;
		else
			RegisterCallbacks();
	}

	private void OnDisable()
	{
		inputManager.RemoveListeners(this);
	}

	private void RegisterCallbacks()
	{
		void callback(InputAction.CallbackContext ctx) => HandleEscPress();
		inputManager.AddListener(inputManager.Input.UI.Back, callback);
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

	public void OnOptionsPress()
	{
		AssetManager.Instance.GetManager<OptionsManager>().ToggleVisiblity();
	}

	public void OnMainMenuPress()
	{
		Time.timeScale = 1f;
		AssetManager.Instance.GetManager<GameManager>().LoadAreaAsync
			(AssetManager.Instance.LoadArea(AssetManager.Instance.Paths.MainMenu));
	}

	public void OnExitPress()
	{
		AssetManager.Instance.GetManager<GameManager>().ExitGame();
	}
	#endregion
}
