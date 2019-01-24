using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManagerAnchor : MonoBehaviour 
{
	private MusicManager musicManager;

	private void Awake()
	{
		musicManager = new MusicManager(GetComponent<AudioSource>());
		DontDestroyOnLoad(this);
	}
}
