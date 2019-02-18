using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : Manager
{
	private readonly AudioManagerAnchor anchor;
	private readonly AudioSource audioSource;

	private List<AudioClip> currentPlaylist;
	private int currentTrackIndex;

	private float currentTrackLength;
	private float currentTrackTime;

	public AudioManager(AudioManagerAnchor anchor)
	{
		this.anchor = anchor;
		audioSource = anchor.audioSource;

		anchor.StartCoroutine(PlaylistChecker());
	}

	public float GetActualVolume(float convertedVolume)
	{
		return - convertedVolume * convertedVolume;
	}

	public float GetConvertedVolume(float actualVolume)
	{
		return - Mathf.Sqrt(Mathf.Abs(actualVolume));
	}

	public void SetMusicVolume(float convertedVol)
	{
		anchor.MusicMixer.SetFloat("musicVol", convertedVol);
	}

	public void SetEffectVolume(float convertedVol)
	{
		anchor.EffectMixer.SetFloat("effectVol", convertedVol);
	}

	public float GetMusicVolume()
	{
		anchor.MusicMixer.GetFloat("musicVol", out float volume);
		return GetConvertedVolume(volume);
	}

	public float GetEffectVolume()
	{
		anchor.EffectMixer.GetFloat("effectVol", out float volume);
		return GetConvertedVolume(volume);
	}

	public void SetNewPlaylist(AudioClip[] musicTracks)
	{
		if(musicTracks != null && musicTracks.Length > 0)
		{
			currentPlaylist = new List<AudioClip>(musicTracks);
			currentTrackIndex = Random.Range(0, currentPlaylist.Count);
			audioSource.Stop();
			currentTrackTime = currentTrackLength; // force skip to next track
		}
		else
		{
			Debug.Log("Empty playlist set.");
			currentPlaylist = null;
		}
	}

	private IEnumerator PlaylistChecker()
	{
		while (true)
		{
			while(currentTrackTime < currentTrackLength)
			{
				currentTrackTime += Time.deltaTime;
				yield return null;
			}
			if (currentPlaylist != null) PlayNextTrack();
			else yield return null;
		}
	}

	private void PlayNextTrack()
	{
		var clip = currentPlaylist[(++currentTrackIndex) % currentPlaylist.Count];
		currentTrackLength = clip.length;
		currentTrackTime = 0f;

		audioSource.PlayOneShot(clip);
	}
	
	public void FadeToNewPlaylist(AudioClip[] musicTracks)
	{
		anchor.StartCoroutine(StartNewPlaylistFade(musicTracks));
	}

	private IEnumerator StartNewPlaylistFade(AudioClip[] musicTracks)
	{
		AnimationCurve fadeCurve = AssetManager.Instance.LoadBundle<AudioSettings>(AssetManager.Instance.Paths.SettingsPath, "Audio Settings").FadeCurve;
		float fadeTimer = 0f;
		float smoothingDuration = fadeCurve.keys[fadeCurve.length - 1].time;

		// smoothing out track end
		while (fadeTimer < smoothingDuration * .5f)
		{
			fadeTimer += Time.deltaTime;
			audioSource.volume = fadeCurve.Evaluate(fadeTimer);
			yield return null;
		}
		
		SetNewPlaylist(musicTracks);

		// smoothing in track begin
		while (fadeTimer < smoothingDuration)
		{
			fadeTimer += Time.deltaTime;
			audioSource.volume = fadeCurve.Evaluate(fadeTimer);
			yield return null;
		}
		yield return null;
	}

	private IEnumerator StartFadeout()
	{

		yield return null;
	}
}
