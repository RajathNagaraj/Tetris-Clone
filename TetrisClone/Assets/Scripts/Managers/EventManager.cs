using System;
using UnityEngine;

public static class EventManager
{
    public static Action OnRowCompleted = delegate { };
    public static Action<int> OnAllRowsCleared = delegate { };
    public static Action<int> OnLevelUp = delegate { };
    public static Action OnLevelUpNotifyUI = delegate { };
    public static Action<int, int, int> OnUpdateScoreUI = delegate { };

}
