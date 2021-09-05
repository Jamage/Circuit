using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase()]
public class CircuitPoint : MonoBehaviour, IEquatable<IBoardObject>, IBoardObject
{
    public string Name { get; private set; }
    public Vector2Int PositionIndex { get; private set; }
    public Edge Edge { get => edge; set => edge = value; }
    [SerializeField] private Edge edge;
    [SerializeField] private bool UseEdge = true;
    public bool IsConnected { get; set; }
    [HideInInspector] public List<Vector2Int> LinePoints { get; private set; }
    [HideInInspector] public BoardObjectType BoardObjectType { get; private set; }
    [HideInInspector] public int RequiredConnections { get; private set; }
    public Dictionary<IBoardObject, List<Vector2Int>> ConnectedObjectsAt { get; private set; }

    private void Awake()
    {
        Name = name;
        IsConnected = false;
        LinePoints = new List<Vector2Int>();
        ConnectedObjectsAt = new Dictionary<IBoardObject, List<Vector2Int>>();
        BoardObjectType = BoardObjectType.Circuit;
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
        SetPosition();
    }

    public void InitializeInner()
    {
        Edge = Helper.GetRandomEnum<Edge>();
        UseEdge = false;
        SetPosition();
    }

    private void SetPosition()
    {
        do
        {
            SetIndex();
        }
        while (CircuitManager.IsAvailable(this) == false);

        transform.position = GameBoardController.GetPositionFor(PositionIndex);
    }

    private void SetIndex()
    {
        if (UseEdge)
            PositionIndex = GameBoardController.GetIndexFor(Edge);
        else
            PositionIndex = GameBoardController.GetInnerIndexPosition();
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