using UnityEngine;

public class MainMenuPanel : MonoBehaviour, IUIPanel
{
	public void OnContinuePress()
	{
		AssetManager.Instance.Load();
	}

	public void OnNewGamePress()
	{
		AssetManager.Instance.CreateNewSavestate();
	}

	public void OnExitPress()
	{
		AssetManager.Instance.Load();
	}

	public void Destroy()
	{
		Destroy(this);
	}

	public void SetActive(bool activeState)
	{
		gameObject.SetActive(activeState);
	}
}