using UnityEngine;

public class MusicManager : Manager 
{
	private AudioSource audioSource;
	public MusicManager(AudioSource audioSource)
	{
		this.audioSource = audioSource;
	}

	public void SetNewPlaylist(AudioClip[] musicTracks)
	{

	}
}
