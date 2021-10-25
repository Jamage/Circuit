using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingPoint : MonoBehaviour, IBoardObject
{
    public Vector2Int PositionIndex { get; private set; }
    public bool IsConnected { get; set; }
    public Dictionary<IBoardObject, List<Vector2Int>> ConnectedObjectsAt { get; private set; }
    public List<Vector2Int> LinePoints => throw new System.NotImplementedException();
    public BoardObjectType BoardObjectType => BoardObjectType.Blocker;
    public int RequiredConnections => 0;
    public string Name => name;
    public Edge Edge { get => edge; set => edge = value; }
    [SerializeField] private Edge edge;
    [SerializeField] private bool UseEdge = true;
    public SpriteRenderer[] spriteRenderers;
    [SerializeField] private Color disconnectedColor = Color.green;
    [SerializeField] private Color connectedColor;

    private void Awake()
    {
        ConnectedObjectsAt = new Dictionary<IBoardObject, List<Vector2Int>>();
    }

    private void OnEnable()
    {
        CircuitManager.allBlockingPoints.Add(this);
        BoardObjectManager.allBoardObjects.Add(this);
    }

    private void OnDisable()
    {
        CircuitManager.allBlockingPoints.Remove(this);
        BoardObjectManager.allBoardObjects.Remove(this);
    }

    //private void Initialize()
    //{
    //    PositionIndex = GameBoardController.GetRandomIndexPosition();
    //}

    public static BlockingPoint New(BlockingData blockingData)
    {
        BlockingPoint prefab = CircuitManager.GetBlockingPrefab();
        BlockingPoint newPoint = Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
        newPoint.PositionIndex = blockingData.PositionIndex;
        newPoint.transform.position = GameBoardController.GetPositionFor(newPoint.PositionIndex);
        return newPoint;
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

    internal void InitializeInner()
    {
        Edge = Helper.GetRandomEnum<Edge>();
        UseEdge = false;
        SetRandomPosition();
    }

    public void UpdateConnectionStatus()
    {
        if (ConnectedObjectsAt.Count > RequiredConnections)
            Connect();
        else
            Disconnect();
    }

    private void Connect()
    {
        IsConnected = true;
        foreach(SpriteRenderer renderer in spriteRenderers)
        {
            renderer.color = connectedColor;
        }
    }

    private void Disconnect()
    {
        IsConnected = false;
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.color = disconnectedColor;
        }
    }

    private void SetRandomPosition()
    {
        byte maxAttempts = 36;
        byte setAttempts = 0;
        do
        {
            SetRandomIndex();
            setAttempts++;
        }
        while (CircuitManager.IsAvailable(this) == false && setAttempts < maxAttempts);

        if (setAttempts == maxAttempts)
        {
            Destroy(this.gameObject);
            Debug.LogWarning($"Failed to generate Blocking Point, no positions available");
            return;
        }
         
        transform.position = GameBoardController.GetPositionFor(PositionIndex);
    }

    private void SetRandomIndex()
    {
        if (UseEdge)
            PositionIndex = GameBoardController.GetRandomIndexFor(Edge);
        else
            PositionIndex = GameBoardController.GetRandomInnerIndexPosition();
    }
}
