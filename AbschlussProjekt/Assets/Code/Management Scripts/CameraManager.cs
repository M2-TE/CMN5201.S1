using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    public float horizontalResolution = 1920;
    private Camera mainCam;

    private void Start()
    {
        mainCam = AssetManager.Instance.MainCam;
    }

    void OnGUI()
    {
        float currentAspect = (float)Screen.width / (float)Screen.height;
        mainCam.orthographicSize = horizontalResolution / currentAspect / 200f;
    }
}
