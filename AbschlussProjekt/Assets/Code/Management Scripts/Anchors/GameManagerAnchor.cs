using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManagerAnchor : MonoBehaviour 
{
	private GameManager gameManager;
	private InputManager inputManager;

	private void Awake()
	{
		AssetManager instance = AssetManager.Instance;
		gameManager = instance.GetManager<GameManager>() ?? new GameManager();

		InputMaster inputMaster = instance.LoadBundle<VitalAssets>(instance.Paths.VitalAssetsPath, "Vital Assets").InputMaster;
		inputManager = instance.GetManager<InputManager>() ?? new InputManager(inputMaster);
		


		// DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG
		AssetManager.Instance.CreateNewSavestate(); // DEBUG
		AssetManager.Instance.Load(); // DEBUG DEBUG DEBUG
		// DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG DEBUG
	}

	private void OnApplicationQuit()
	{
		gameManager.OnApplicationQuit();
	}
}
