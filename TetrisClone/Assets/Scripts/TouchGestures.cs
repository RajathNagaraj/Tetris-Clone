using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchGestures : MonoBehaviour
{

    private bool isFastFalling;
    private Vector2 touchStartPos;
    public float minSwipeDist = 50f;  // pixels
    public float maxTapDist = 20f;    // for rotate tap

    public void SetupTouch(PlayerControls controls)
    {
        controls.Gameplay.Touch.started += OnTouchStart;
        controls.Gameplay.Touch.canceled += OnTouchEnd;
    }


    void OnTouchStart(InputAction.CallbackContext ctx)
    {
        var primary = Touchscreen.current.primaryTouch;
        touchStartPos = primary.position.ReadValue();
    }

    void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        var primary = Touchscreen.current.primaryTouch;
        Vector2 touchEndPos = primary.position.ReadValue();
        Vector2 delta = touchEndPos - touchStartPos;
        float dist = delta.magnitude;

        ProcessSwipeAction(delta, dist);
    }

    void ProcessSwipeAction(Vector2 delta, float dist)
    {
        if (Mathf.Abs(delta.x) > minSwipeDist && dist > maxTapDist)
        {
            if (delta.x < 0) GameEvents.OnMoveShapeLeft?.Invoke();
            else GameEvents.OnMoveShapeRight?.Invoke();
        }
    }
}
