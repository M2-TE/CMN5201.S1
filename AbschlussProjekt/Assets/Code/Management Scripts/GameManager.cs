using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    private void Start ()
    {
        BaseCharacterController.characterControlEnabled = true;
        Instantiate(AssetManager.Instance.PlayableCharacters.LoadAsset<PlayableCharacter>("Gunwoman").Prefab);
	}
}