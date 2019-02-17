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
		Color ambientColor = RenderSettings.ambientLight;
		var brightnes = brightnessSlider.value;
	}
}
