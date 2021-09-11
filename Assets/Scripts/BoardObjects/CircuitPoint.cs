using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase(), Serializable]
public class CircuitPoint : MonoBehaviour, IEquatable<IBoardObject>, IBoardObject
{
    public string Name => name;
    public Vector2Int PositionIndex { get; private set; }
    public Edge Edge { get => edge; set => edge = value; }
    [SerializeField] private Edge edge;
    [SerializeField] private bool UseEdge = true;
    public bool IsConnected { get; set; }
    [HideInInspector] public List<Vector2Int> LinePoints { get; private set; }
    [HideInInspector] public BoardObjectType BoardObjectType => BoardObjectType.Circuit;
    [HideInInspector] public int RequiredConnections { get; private set; }
    public Dictionary<IBoardObject, List<Vector2Int>> ConnectedObjectsAt { get; private set; }

    private void Awake()
    {
        IsConnected = false;
        LinePoints = new List<Vector2Int>();
        ConnectedObjectsAt = new Dictionary<IBoardObject, List<Vector2Int>>();
        RequiredConnections = 1;
    }

    private void OnEnable()
    {
        CircuitManager.allCircuitPoints.Add(this);
        BoardObjectManager.allBoardObjects.Add(this);
    }

    private void OnDisable()
    {
        CircuitManager.allCircuitPoints.Remove(this);
        BoardObjectManager.allBoardObjects.Remove(this);
    }

    public void InitializeEdge()
    {
        Edge = Helper.GetRandomEnum<Edge>();
        UseEdge = true;
        SetRandomPosition();
    }

    public void InitializeInner()
    {
        Edge = Helper.GetRandomEnum<Edge>();
        UseEdge = false;
        SetRandomPosition();
    }

    private void SetRandomPosition()
    {
        do
        {
            SetRandomIndex();
        }
        while (CircuitManager.IsAvailable(this) == false);

        transform.position = GameBoardController.GetPositionFor(PositionIndex);
    }

    internal static CircuitPoint New(CircuitData circuitData)
    {
        CircuitPoint circuitPointPrefab = CircuitManager.GetCircuitPrefab();
        CircuitPoint newPoint = Instantiate(circuitPointPrefab, Vector3.zero, circuitPointPrefab.transform.rotation);
        newPoint.PositionIndex = circuitData.PositionIndex;
        newPoint.transform.position = GameBoardController.GetPositionFor(newPoint.PositionIndex);
        newPoint.RequiredConnections = circuitData.RequiredConnections;
        return newPoint;
    }

    private void SetRandomIndex()
    {
        if (UseEdge)
            PositionIndex = GameBoardController.GetRandomIndexFor(Edge);
        else
            PositionIndex = GameBoardController.GetRandomInnerIndexPosition();
    }

    public bool TryGetConnectedTo(IBoardObject placedObject, out List<Vector2Int> connectedOn)
    {
        bool isConnected = false;
        connectedOn = new List<Vector2Int>();
        if (placedObject.LinePoints.Contains(PositionIndex))
        {
            connectedOn.Add(PositionIndex);
            isConnected = true;
        }

        return isConnected;
    }

    public void UpdateConnectionStatus()
    {
        if (ConnectedObjectsAt.Count >= RequiredConnections)
            IsConnected = true;
        else
            IsConnected = false;
    }

    public void AddConnection(IBoardObject connectedObject, List<Vector2Int> connectedOn)
    {
        if (ConnectedObjectsAt.ContainsKey(connectedObject))
            return;

        ConnectedObjectsAt.Add(connectedObject, connectedOn);
        UpdateConnectionStatus();
    }

    public void RemoveConnection(IBoardObject connectedObject)
    {
        ConnectedObjectsAt.Remove(connectedObject);
        UpdateConnectionStatus();
    }

    public bool Equals(IBoardObject other)
    {
        return other.BoardObjectType == BoardObjectType
            && other.PositionIndex == PositionIndex
            && other.Name == this.Name;
    }
}

public enum Edge
{
    Left,
    Top,
    Right,
    Bottom
}