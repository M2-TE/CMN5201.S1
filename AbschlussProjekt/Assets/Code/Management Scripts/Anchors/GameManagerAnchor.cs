using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManagerAnchor : MonoBehaviour 
{
	private GameManager gameManager;
	private InputManager inputManager;
	private MusicManager musicManager;

	private void Awake()
	{
		AssetManager instance = AssetManager.Instance;
		gameManager = instance.GetManager<GameManager>() ?? new GameManager();
		musicManager = instance.GetManager<MusicManager>() ?? new MusicManager(GetComponent<AudioSource>());

		InputMaster inputMaster = instance.LoadBundle<VitalAssets>(instance.Paths.VitalAssetsPath, "Vital Assets").InputMaster;
		inputManager = instance.GetManager<InputManager>() ?? new InputManager(inputMaster);
	}

	private void OnApplicationQuit()
	{
		gameManager.OnApplicationQuit();
	}
}
