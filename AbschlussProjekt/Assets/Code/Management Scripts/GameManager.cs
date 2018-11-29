using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public AssetContainer assetContainer;
    
    private void Start ()
    {
        BaseCharacterController.characterControlEnabled = false;

        Instantiate(assetContainer.Mage.Prefab);
        assetContainer.checkValue++;
	}
}