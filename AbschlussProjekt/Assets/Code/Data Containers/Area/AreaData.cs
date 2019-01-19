using UnityEngine;

public abstract class AreaData : DataContainer
{
	[SerializeField] private string sceneName;
	public string SceneName
	{
		get { return (sceneName == "") ? name : sceneName; }
	}

	public AudioClip[] MusicPool;
}
