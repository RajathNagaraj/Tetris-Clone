using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchGestures : MonoBehaviour
{

    public float minSwipeDist = 50f;  // pixels
    public float maxSwipeTime = 0.35f;
    private float longPressMoveTolerancePx = 25f;

    private int activeFingerIndex = -1;   // -1 = no finger locked
    private FingerState s;
    private double longPressDuration = 0.5f;



    struct FingerState
    {
        public Vector2 startPos;
        public double startTime;
        public bool longPressStarted;
        public bool movedTooMuchForLongPress;
    }


    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerMove += OnFingerMove;
        Touch.onFingerUp += OnFingerUp;
    }

    void OnFingerUp(Finger finger)
    {
        if (finger.index != activeFingerIndex) return;

        var t = finger.currentTouch;
        var dt = t.time - s.startTime;
        var delta = t.screenPosition - s.startPos;

        if (s.longPressStarted)
        {
            OnLongPressEnd();
            ResetLock();
            return;
        }

        if (dt <= maxSwipeTime &&
            Mathf.Abs(delta.x) >= minSwipeDist &&
            Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0) OnSwipeRight();
            else OnSwipeLeft();
            ResetLock();
            return;
        }

        if (t.isTap) OnTap();
        ResetLock();
    }

    private void OnLongPressEnd()
    {
        GameEvents.OnShapeFallFaster?.Invoke(false);
    }

    private void OnLongPressStart()
    {
        GameEvents.OnShapeFallFaster?.Invoke(true);
    }

    private void OnSwipeLeft()
    {
        GameEvents.OnMoveShapeLeft?.Invoke();
    }

    private void OnSwipeRight()
    {
        GameEvents.OnMoveShapeRight?.Invoke();
    }

    private void OnTap()
    {
        GameEvents.OnRotateShape?.Invoke();
    }

    void ResetLock()
    {
        activeFingerIndex = -1;
        s = default;
    }

    private void OnFingerMove(Finger finger)
    {
        if (finger.index != activeFingerIndex) return;

        var t = finger.currentTouch;

        if (!s.movedTooMuchForLongPress)
        {
            if ((t.screenPosition - s.startPos).sqrMagnitude >
                longPressMoveTolerancePx * longPressMoveTolerancePx)
                s.movedTooMuchForLongPress = true;
        }
    }

    private void OnFingerDown(Finger finger)
    {
        if (activeFingerIndex != -1) return;

        activeFingerIndex = finger.index;
        var t = finger.currentTouch;

        s = new FingerState
        {
            startPos = t.screenPosition,
            startTime = t.startTime,
            longPressStarted = false,
            movedTooMuchForLongPress = false
        };
    }

    void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerMove -= OnFingerMove;
        Touch.onFingerUp -= OnFingerUp;
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (activeFingerIndex == -1) return;

        // Find the currently active finger that matches our locked index.
        // Touch.activeFingers is the set of fingers currently touching the screen. 
        Finger lockedFinger = null;
        foreach (var f in Touch.activeFingers)
            if (f.index == activeFingerIndex) { lockedFinger = f; break; }

        if (lockedFinger == null) return;

        if (s.longPressStarted || s.movedTooMuchForLongPress) return;

        var t = lockedFinger.currentTouch;
        if (t.time - s.startTime >= longPressDuration)
        {
            s.longPressStarted = true;
            OnLongPressStart();
        }
    }


}




