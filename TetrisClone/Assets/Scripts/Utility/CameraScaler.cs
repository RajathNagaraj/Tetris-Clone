using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    [Tooltip("Desired visible world width, in units.")]
    public float designWorldWidth = 15f;
    [Tooltip("Desired visible world height, in units.")]
    public float designWorldHeight = 30f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();


#if UNITY_ANDROID
        ApplyAndroidFixedWidth();
#elif UNITY_STANDALONE_WIN
        ApplyWindowsFixedHeight();
#else
        // Fallback: choose one behavior for other platforms
        ApplyWindowsFixedHeight();
#endif
    }
    void ApplyAndroidFixedWidth()
    {
        cam = GetComponent<Camera>();

        float aspect = (float)Screen.width / Screen.height;
        // visibleWidth = visibleHeight * aspect = (orthographicSize * 2) * aspect
        // => orthographicSize = (designWidth / aspect) / 2
        float desiredOrthoSize = (designWorldWidth / aspect) * 0.5f;

        cam.orthographicSize = desiredOrthoSize;
    }
    void ApplyWindowsFixedHeight()
    {
        // visibleHeight = orthographicSize * 2
        cam.orthographicSize = designWorldHeight * 0.5f;
    }
}
