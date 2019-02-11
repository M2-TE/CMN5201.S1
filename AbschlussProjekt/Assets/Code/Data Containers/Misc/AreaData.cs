using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

[CreateAssetMenu(fileName = "New Area", menuName = "Data Container/Area")]
public class AreaData : DataContainer
{
	public SceneField Scene;

	public AudioClip[] MusicPool;

	public override string ToString()
	{
		return name;
	}
}
