using UnityEngine;

public class GameManagerAnchor : MonoBehaviour 
{
	private GameManager gameManager;
	
	private void Start()
	{
		gameManager = AssetManager.Instance.GetManager<GameManager>() ?? new GameManager();
	}

	private void OnApplicationQuit()
	{
		gameManager.OnApplicationQuit();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) gameManager.StartCombatDebugging();
	}
}
