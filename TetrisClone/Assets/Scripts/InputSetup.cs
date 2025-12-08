using UnityEngine;

public class InputSetup : MonoBehaviour
{
    PlayerControls m_controls;
    public void SetupControls(PlayerControls controls)
    {
        m_controls = controls;
        m_controls.Gameplay.MoveLeft.performed += ctx => GameEvents.OnMoveShapeLeft?.Invoke();
        m_controls.Gameplay.MoveRight.performed += ctx => GameEvents.OnMoveShapeRight?.Invoke();
        m_controls.Gameplay.Rotate.performed += ctx => GameEvents.OnRotateShape?.Invoke();
        m_controls.Gameplay.FallFaster.started += ctx => GameEvents.OnShapeFallFaster?.Invoke(true);
        m_controls.Gameplay.FallFaster.canceled += ctx => GameEvents.OnShapeFallFaster?.Invoke(false);
    }


    void OnDestroy()
    {
        m_controls.Gameplay.MoveLeft.performed -= ctx => GameEvents.OnMoveShapeLeft?.Invoke();
        m_controls.Gameplay.MoveRight.performed -= ctx => GameEvents.OnMoveShapeRight?.Invoke();
        m_controls.Gameplay.Rotate.performed -= ctx => GameEvents.OnRotateShape?.Invoke();
        m_controls.Gameplay.FallFaster.started -= ctx => GameEvents.OnShapeFallFaster?.Invoke(true);
        m_controls.Gameplay.FallFaster.canceled -= ctx => GameEvents.OnShapeFallFaster?.Invoke(false);
    }

}
