using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManagerAnchor : MonoBehaviour
{
	[NonSerialized] public AudioSource audioSource;
	public AudioMixer MusicMixer;
	public AudioMixer EffectMixer;

	private AudioManager musicManager;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		DontDestroyOnLoad(this);
	}

	private void Start()
	{
		var instance = AssetManager.Instance;
		musicManager = instance.GetManager<AudioManager>() ?? new AudioManager(this);

		var runtimeSettings = instance.Savestate.RuntimeSettings;
		if (runtimeSettings == null) runtimeSettings = new RuntimeSettings();

		musicManager.SetEffectVolume((1f - runtimeSettings.EffectVolume) * -40f);
		musicManager.SetMusicVolume((1f - runtimeSettings.MusicVolume) * -40f);
		musicManager.SetNewPlaylist(instance.GetManager<GameManager>().CurrentArea.MusicPool);
	}
}
