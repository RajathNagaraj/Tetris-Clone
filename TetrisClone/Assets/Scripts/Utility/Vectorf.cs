using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vectorf 
{
    public static Vector2 Round(Vector2 initialVector)
    {
        return new Vector2(Mathf.Round(initialVector.x), Mathf.Round(initialVector.y));
    }
}
