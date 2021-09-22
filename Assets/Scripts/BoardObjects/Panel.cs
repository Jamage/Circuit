using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase()]
public class Panel : MonoBehaviour, IEquatable<IBoardObject>, IBoardObject
{
    public String Name => name;
    public PanelType panelType = PanelType.OneTwo;
    public LineRenderer[] borderLines = new LineRenderer[4];
    public LineRenderer[] innerLines = new LineRenderer[2];
    public Vector2Int PositionIndex { get; private set; }
    public List<Vector2Int> LinePoints { get; private set; }
    public bool IsConnected { get; set; }
    public BoardObjectType BoardObjectType => BoardObjectType.Panel;
    public int RequiredConnections { get; private set; }
    public Dictionary<IBoardObject, List<Vector2Int>> ConnectedObjectsAt { get; private set; }

    public static UnityAction<IBoardObject> OnPlacement;
    private Vector3 startPosition;
    private Vector2 dragOffsetPoint;
    public Material disconnectedMaterial;
    public Material connectedMaterial;
    public PanelSelectionHighlight selectionHighlight;
    public static bool dragging = false;

    void Awake()
    {
        PositionIndex = new Vector2Int(-1, -1);
        LinePoints = new List<Vector2Int>();
        ConnectedObjectsAt = new Dictionary<IBoardObject, List<Vector2Int>>();
        IsConnected = false;
        RequiredConnections = 2;
        if (selectionHighlight == null)
            selectionHighlight = GetComponentInChildren<PanelSelectionHighlight>();
    }

    private void OnEnable()
    {
        PanelManager.allPanels.Add(this);
        BoardObjectManager.allBoardObjects.Add(this);
    }

    private void OnDisable()
    {
        PanelManager.allPanels.Remove(this);
        BoardObjectManager.allBoardObjects.Remove(this);
    }

    internal static Panel New(PanelData panelData)
    {
        Panel panelPrefab = PanelManager.Get(panelData.PanelType);
        Panel newPanel = Instantiate(panelPrefab, Vector3.zero, panelPrefab.transform.rotation);
        newPanel.Setup(panelData);
        return newPanel;
    }

    private void Setup(PanelData panelData)
    {
        SetPosition(panelData.PositionIndex);
        RemoveFromConnections();
        ClearConnections();
        OnPlacement?.Invoke(this);
    }

    public void OnMouseEnter()
    {
        if (dragging == false)
        {
            selectionHighlight?.MouseEntered();
        }
    }

    private void OnMouseExit()
    {
        if (dragging == false)
        {
            selectionHighlight?.MouseExited();
        }
    }

    public void OnMouseDown()
    {
        selectionHighlight?.MouseExited();
        startPosition = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 20, LayerMask.GetMask("Panel"));
        if (hit.collider != null)
        {
            dragOffsetPoint = (Vector2)transform.position - hit.point;
        }

        dragging = true;
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
            }
            else
            {
                SetPosition(bgPanel.PositionIndex);
                RemoveFromConnections();
                ClearConnections();
                OnPlacement?.Invoke(this);
            }
        }
        else
        {
            transform.position = startPosition;
        }

        StartCoroutine(TurnOffDragging());
    }

    private IEnumerator TurnOffDragging()
    {
        yield return new WaitForSeconds(.04f);
        dragging = false;
        SelectMouseOver();
    }

    private void SelectMouseOver()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 20, LayerMask.GetMask("Panel"));
        if (hit.collider != null)
        {
            Panel mouseOverPanel = hit.collider.GetComponent<Panel>();
            mouseOverPanel.selectionHighlight?.MouseEntered();
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

    private void SetPosition(Vector2Int positionIndex)
    {
        PositionIndex = positionIndex;
        transform.position = GameBoardController.GetPositionFor(PositionIndex);
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
        if (ConnectionCount() >= RequiredConnections && IsNotBlocked())
            Connect();
        else
            Disconnect();
    }

    private bool IsNotBlocked() => IsBlocked() == false;

    private bool IsBlocked()
    {
        bool isTouchingBlocker = false;

        foreach(IBoardObject boardObject in ConnectedObjectsAt.Keys)
        {
            if (boardObject.BoardObjectType == BoardObjectType.Blocker)
                isTouchingBlocker = true;
        }

        return isTouchingBlocker;
    }

    private void Disconnect()
    {
        if (IsConnected == false)
            return;

        IsConnected = false;
        SetDisconnectedMaterial();
    }

    private void SetDisconnectedMaterial()
    {
        foreach(LineRenderer line in innerLines)
        {
            line.material = disconnectedMaterial;
        }
    }

    private void Connect()
    {
        if (IsConnected)
            return;

        IsConnected = true;
        SetConnectedMaterial();
    }

    private void SetConnectedMaterial()
    {
        foreach(LineRenderer line in innerLines)
        {
            line.material = connectedMaterial;
        }
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
