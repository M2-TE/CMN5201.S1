using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;

public class PauseMenuPanel : MonoBehaviour
{
	[SerializeField] private GameObject visiblePanel;
	[SerializeField] private GameObject continueButton;

	private EventSystem eventSystem;

	private void Start()
	{
		eventSystem = GetComponentInChildren<EventSystem>();
		visiblePanel.SetActive(false);

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
		visiblePanel.SetActive(!visiblePanel.activeInHierarchy);
		Time.timeScale = (visiblePanel.activeInHierarchy) ? 0f : 1f;

		if(visiblePanel.activeInHierarchy)
		{
			eventSystem.SetSelectedGameObject(null);
			eventSystem.SetSelectedGameObject(continueButton);
		}
	}

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
}
