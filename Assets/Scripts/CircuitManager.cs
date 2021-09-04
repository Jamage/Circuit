using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitManager : MonoBehaviour
{
    public static List<CircuitPoint> allCircuitPoints = new List<CircuitPoint>();

    internal static List<Vector2Int> GetCircuitIndexes()
    {
        List<Vector2Int> circuitIndexList = new List<Vector2Int>();

        foreach(CircuitPoint point in allCircuitPoints)
        {
            circuitIndexList.Add(point.PositionIndex);
        }

        return circuitIndexList;
    }

    internal static bool AreAllCircuitsConnected()
    {
        bool allConnected = true;

        foreach (CircuitPoint circuit in allCircuitPoints)
        {
            if (circuit.IsConnected == false)
                allConnected = false;
        }

        return allConnected;
    }
}
