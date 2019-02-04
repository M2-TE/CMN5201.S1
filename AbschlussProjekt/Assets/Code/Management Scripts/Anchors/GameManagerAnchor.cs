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
	}

	private void OnApplicationQuit()
	{
		gameManager.OnApplicationQuit();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			gameManager.StartCombatDebugging();
		}
	}
}
