using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase()]
public class Panel : MonoBehaviour, IEquatable<IBoardObject>, IBoardObject
{
    public String Name { get; private set; }
    public PanelType panelType = PanelType.OneTwo;
    public LineRenderer[] borderLines = new LineRenderer[4];
    public LineRenderer[] innerLines = new LineRenderer[2];
    public Vector2Int PositionIndex { get; private set; }
    public List<Vector2Int> LinePoints { get; private set; }
    public bool IsConnected { get; set; }
    public BoardObjectType BoardObjectType { get; private set; }
    public int RequiredConnections { get; private set; }
    public Dictionary<IBoardObject, List<Vector2Int>> ConnectedObjectsAt { get; private set; }

    public static UnityAction<IBoardObject> OnPlacement;

    private Vector3 startPosition;
    private Vector2 dragOffsetPoint;

    void Awake()
    {
        Name = name;
        LinePoints = new List<Vector2Int>();
        ConnectedObjectsAt = new Dictionary<IBoardObject, List<Vector2Int>>();
        IsConnected = false;
        BoardObjectType = BoardObjectType.Panel;
        RequiredConnections = 2;
    }

    private void OnEnable()
    {
        PanelManager.allPanels.Add(this);
        BoardObjectManager.allBoardObjects.Add(this);
    }

    private void OnDisable()
    {
        PanelManager.allPanels.Remove(this);
    }

    public void OnMouseDown()
    {
        startPosition = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 20, LayerMask.GetMask("Panel"));
        if (hit.collider != null)
        {
            dragOffsetPoint = (Vector2)transform.position - hit.point;
        }
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(mousePos + (Vector3.forward * 10)) + dragOffsetPoint;
    }

    public void OnMouseUp()
    {
        RaycastHit2D bgHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 20, LayerMask.GetMask("Background"));
        if (bgHit.collider != null)
        {
            Vector3 newPos = bgHit.transform.position;
            newPos.z = 0;
            BackgroundPanel bgPanel = bgHit.collider.GetComponent<BackgroundPanel>();
            
            if (PanelManager.IsOccupied(bgPanel.PositionIndex))
            {
                transform.position = startPosition;
                return;
            }

            transform.position = newPos;
            SetPositionIndex(bgPanel);
            RemoveFromConnections();
            ClearConnections();
            OnPlacement?.Invoke(this);
        }
        else
        {
            transform.position = startPosition;
        }
    }

    private void ClearConnections()
    {
        ConnectedObjectsAt.Clear();
        UpdateConnectionStatus();
    }

    private void RemoveFromConnections()
    {
        foreach(IBoardObject connection in ConnectedObjectsAt.Keys)
        {
            connection.RemoveConnection(this);
        }
    }

    private void SetPositionIndex(BackgroundPanel backgroundPanel)
    {
        PositionIndex = backgroundPanel.PositionIndex;
        LinePoints.Clear();
        LinePoints.AddRange(GameBoardController.GetConnectingPoints(PositionIndex, panelType));
    }

    public bool Equals(IBoardObject other)
    {
        return other.BoardObjectType == BoardObjectType
            && other.PositionIndex == PositionIndex
            && other.Name == this.Name;
    }

    public bool TryGetConnectedTo(IBoardObject placedObject, out List<Vector2Int> connectedOn)
    {
        bool isConnected = false;
        connectedOn = new List<Vector2Int>();

        foreach(Vector2Int point in LinePoints)
        {
            if (placedObject.LinePoints.Contains(point))
            {
                connectedOn.Add(point);
                isConnected = true;
            }
        }

        return isConnected;
    }

    public void UpdateConnectionStatus()
    {
        if (ConnectionCount() >= RequiredConnections)
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

    private int ConnectionCount()
    {
        List<Vector2Int> uniquePoints = new List<Vector2Int>();
        foreach(IBoardObject boardObject in ConnectedObjectsAt.Keys)
        {
            foreach(Vector2Int connectedPoint in ConnectedObjectsAt[boardObject])
            {
                if (uniquePoints.Contains(connectedPoint) == false)
                    uniquePoints.Add(connectedPoint);
            }
        }
        return uniquePoints.Count;
    }
}

public enum PanelType
{
    OneTwo,
    OneThree,
    OneFour,
    TwoThree,
    TwoFour,
    ThreeFour
}
