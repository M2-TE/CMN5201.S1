using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    public float horizontalResolution = 1920;

    void OnGUI()
    {
        float currentAspect = (float)Screen.width / (float)Screen.height;
        mainCam.orthographicSize = horizontalResolution / currentAspect / 200f;
    }
}
