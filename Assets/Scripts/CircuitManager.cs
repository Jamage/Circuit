using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitManager : MonoBehaviour
{
    public CircuitPoint circuitPrefab;
    public static CircuitPoint staticCircuitPrefab;
    public BlockingPoint blockingPrefab;
    public static BlockingPoint staticBlockingPrefab;
    public static List<CircuitPoint> allCircuitPoints = new List<CircuitPoint>();
    public static List<BlockingPoint> allBlockingPoints = new List<BlockingPoint>();

    private void Awake()
    {
        staticCircuitPrefab = circuitPrefab;
        staticBlockingPrefab = blockingPrefab;
    }

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

    internal static bool IsAvailable(IBoardObject point)
    {
        bool isAvailable = true;
        foreach(IBoardObject circuit in allCircuitPoints)
        {
            if (point == circuit)
                continue;

            if(point.PositionIndex == circuit.PositionIndex)
            {
                isAvailable = false;
                break;
            }
        }

        foreach (IBoardObject block in allBlockingPoints)
        {
            if (point == block)
                continue;

            if (point.PositionIndex == block.PositionIndex)
            {
                isAvailable = false;
                break;
            }
        }

        return isAvailable;
    }

    internal static bool NoBlockingPointsConnected()
    {
        bool notConnected = true;
        foreach(BlockingPoint blockingPoint in allBlockingPoints)
        {
            if (blockingPoint.IsConnected)
                notConnected = false;
        }

        return notConnected;
    }

    internal static CircuitPoint GetCircuitPrefab()
    {
        return staticCircuitPrefab;
    }

    internal static BlockingPoint GetBlockingPrefab()
    {
        return staticBlockingPrefab;
    }
}
