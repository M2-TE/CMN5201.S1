using System.Collections;
using UnityEngine;

public class LoadingScreenManager : Manager
{
	private LoadingScreenPanel panel;
	private bool sceneDarkened;

	public void Register(LoadingScreenPanel panel)
	{
		this.panel = panel;
		HideLoadingScreen();
	}

	public void ShowLoadingScreen()
	{
		panel.StartCoroutine(HideScene());
	}
	public void HideLoadingScreen()
	{
		panel.StartCoroutine(RevealScene());
	}

	private IEnumerator RevealScene()
	{
		sceneDarkened = false;

		var color = panel.CoverImage.color;
		panel.CoverImage.color = new Color(color.r, color.r, color.r, 1f);
		yield return null;

		var timer = 0f;
		var instance = AssetManager.Instance;
		var fadeTimer = instance.LoadBundle<MiscSettings>(instance.Paths.SettingsPath, "Misc Settings").loadingScreenFadeDuration;
		while (timer < fadeTimer && !sceneDarkened)
		{
			panel.CoverImage.color = new Color(color.r, color.r, color.r, 1 - 1f / fadeTimer * timer);
			timer += Time.deltaTime;
			yield return null;
		}

		if(!sceneDarkened) panel.CoverImage.color = new Color(color.r, color.r, color.r, 0f); // if the coroutine is still valid, set cover image to final state: revealing scene fully
	}

	private IEnumerator HideScene()
	{
		sceneDarkened = true;

		var color = panel.CoverImage.color;
		panel.CoverImage.color = new Color(color.r, color.r, color.r, 0f);
		yield return null;

		var timer = 0f;
		var instance = AssetManager.Instance;
		var fadeTimer = instance.LoadBundle<MiscSettings>(instance.Paths.SettingsPath, "Misc Settings").loadingScreenFadeDuration;
		while (timer < fadeTimer && sceneDarkened)
		{
			panel.CoverImage.color = new Color(color.r, color.r, color.r, 1f / fadeTimer * timer);
			timer += Time.deltaTime;
			yield return null;
		}

		if (sceneDarkened) panel.CoverImage.color = new Color(color.r, color.r, color.r, 1f); // similar to RevealScene()
	}
}
