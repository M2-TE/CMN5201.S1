using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManagerAnchor : MonoBehaviour
{
	[SerializeField] private GameObject MusicManagerPrefab;
	private GameManager gameManager;
	private InputManager inputManager;

	private void Awake()
	{
		AssetManager instance = AssetManager.Instance;
		gameManager = instance.GetManager<GameManager>() ?? new GameManager();

		InputMaster inputMaster = instance.LoadBundle<VitalAssets>(instance.Paths.VitalAssetsPath, "Vital Assets").InputMaster;
		inputManager = instance.GetManager<InputManager>() ?? new InputManager(inputMaster);

		//if(instance.GetManager<AudioManager>() == null)
	}

	private void OnApplicationQuit()
	{
		gameManager.OnApplicationQuit();
	}
}
