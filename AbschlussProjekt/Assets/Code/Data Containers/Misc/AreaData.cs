using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

[CreateAssetMenu(fileName = "New Area", menuName = "Data Container/Area Data")]
public class AreaData : DataContainer 
{
	[SerializeField] private string sceneName;
	public string SceneName
	{
		get { return (sceneName == "") ? name : sceneName; }
	}
	public AudioClip[] MusicPool;
}
