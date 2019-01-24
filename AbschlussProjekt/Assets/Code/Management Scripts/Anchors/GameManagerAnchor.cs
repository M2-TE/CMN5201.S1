using UnityEngine;

public class GameManagerAnchor : MonoBehaviour 
{
	private GameManager gameManager;

	private void Awake()
	{
		try { gameManager = AssetManager.Instance.GetManager<GameManager>(); }
		catch { gameManager = new GameManager(); }
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
