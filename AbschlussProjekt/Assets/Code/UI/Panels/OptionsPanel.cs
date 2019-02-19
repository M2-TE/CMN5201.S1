using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : UIPanel
{
	public Slider brightnessSlider, musicVolumeSlider, effectVolumeSlider;

	private AudioManager _audioManager;
	private AudioManager audioManager
	{
		get { return _audioManager ?? (_audioManager = AssetManager.Instance.GetManager<AudioManager>()); }
	}

	private void Start()
	{
		float brightness = AssetManager.Instance.Savestate.RuntimeSettings.Brightness;
		RenderSettings.ambientLight = new Color(brightness, brightness, brightness);
		brightnessSlider.value = brightness;
	}

	private void OnEnable()
	{
		var manager = AssetManager.Instance.GetManager<OptionsManager>() ?? new OptionsManager(this);
		manager.Register(this);
	}

	public void AdjustMusicVolume()
	{
		audioManager.SetMusicVolume(audioManager.GetActualVolume(musicVolumeSlider.value));
	}

	public void AdjustEffectVolume()
	{
		audioManager.SetEffectVolume(audioManager.GetActualVolume(effectVolumeSlider.value));
	}

	public void AdjustBrightness()
	{
		RenderSettings.ambientLight = new Color(brightnessSlider.value, brightnessSlider.value, brightnessSlider.value);
		AssetManager.Instance.Savestate.RuntimeSettings.Brightness = brightnessSlider.value;
	}
}
