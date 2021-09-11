using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardObject
{
    Vector2Int PositionIndex { get; }
    bool IsConnected { get; set; }
    List<Vector2Int> LinePoints { get; }
    BoardObjectType BoardObjectType { get; }
    int RequiredConnections { get; }
    string Name { get; }

    void UpdateConnectionStatus();
    bool TryGetConnectedTo(IBoardObject placedObject, out List<Vector2Int> connectedOn);
    Dictionary<IBoardObject, List<Vector2Int>> ConnectedObjectsAt { get; } //Key: Board Object, Value: List of Connection Points
    void AddConnection(IBoardObject connectedObject, List<Vector2Int> connectedOn);
    void RemoveConnection(IBoardObject connectedObject);
}

public enum BoardObjectType
{
    Panel,
    Circuit,
    Blocker
}
