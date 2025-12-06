using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    [Tooltip("Desired visible world width, in units.")]
    public float designWorldWidth = 15;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();

        float aspect = (float)Screen.width / Screen.height;
        // visibleWidth = visibleHeight * aspect = (orthographicSize * 2) * aspect
        // => orthographicSize = (designWidth / aspect) / 2
        float desiredOrthoSize = (designWorldWidth / aspect) * 0.5f;

        cam.orthographicSize = desiredOrthoSize;
    }
}
