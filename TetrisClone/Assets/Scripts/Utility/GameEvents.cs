using System;
using UnityEngine;

public static class GameEvents
{
    public static Action OnRowCompleted = delegate { };
    public static Action<int> OnAllRowsCleared = delegate { };
    public static Action<Vector3> OnSpawnShape = delegate { };
    public static Action<int> OnLevelUp = delegate { };
    public static Action OnLevelUpNotifyUI = delegate { };
    public static Action<int, int, int> OnUpdateScoreUI = delegate { };
    public static Action<int> OnRowGlow = delegate { };
    public static Action<Transform> OnLandShapeGlow = delegate { };
    public static Action OnGameOver = delegate { };
    public static Action OnCrossLineOfDeath = delegate { };

}
