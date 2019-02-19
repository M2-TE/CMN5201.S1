using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : Manager
{
	OptionsPanel panel;

	private AudioManager _audioManager;
	private AudioManager audioManager
	{
		get { return _audioManager ?? (_audioManager = AssetManager.Instance.GetManager<AudioManager>()); }
	}

	public OptionsManager(OptionsPanel panel)
	{
		panel.StartCoroutine(InitSlidersOnDelay());
	}

	private IEnumerator InitSlidersOnDelay()
	{
		yield return null;
		panel.brightnessSlider.value = RenderSettings.ambientLight.r;
		panel.musicVolumeSlider.value = audioManager.GetMusicVolume();
		panel.effectVolumeSlider.value = audioManager.GetEffectVolume();
	}

	public void Register(OptionsPanel panel) { this.panel = panel; }

	public void ToggleVisiblity()
	{
		panel.ToggleVisibility();
	}
}
