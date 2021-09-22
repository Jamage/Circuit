using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CircuitData
{
    public Vector2Int PositionIndex;
    public int RequiredConnections;

    public CircuitData()
    {
        PositionIndex = Vector2Int.zero;
        RequiredConnections = 1;
    }
}
