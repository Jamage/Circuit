using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardObjectManager : MonoBehaviour
{
    public static List<IBoardObject> allBoardObjects = new List<IBoardObject>();

    private void OnEnable()
    {
        Panel.OnPlacement += HandleConnections;
    }

    private void OnDisable()
    {
        Panel.OnPlacement -= HandleConnections;
    }

    public void HandleConnections(IBoardObject placedObject)
    {
        foreach (IBoardObject boardObject in allBoardObjects)
        {
            if (boardObject == placedObject) // Don't handle for connection to self
                continue;

            if (boardObject.TryGetConnectedTo(placedObject, out List<Vector2Int> connectedOn))
            {
                placedObject.AddConnection(boardObject, connectedOn);
                boardObject.AddConnection(placedObject, connectedOn);
            }
        }

        if (AllObjectsAreConnected())
        {
            LevelSelectManager.Instance.SetLevelComplete();
            LevelManager.OnLevelComplete?.Invoke();
        }
    }

    public bool AllObjectsAreConnected()
    {
        bool isConnected = PanelManager.AreAllPanelsConnected() && CircuitManager.AreAllCircuitsConnected() && CircuitManager.NoBlockingPointsConnected();
        return isConnected && AreAllObjectsTogether();
    }

    private bool AreAllObjectsTogether()
    {
        int linkCount = GetLinkCount(allBoardObjects);
        int expectedCount = GetExpectedCount();

        return linkCount == expectedCount;
    }

    private int GetExpectedCount()
    {
        return allBoardObjects.Count - allBoardObjects.Where(x => x.BoardObjectType == BoardObjectType.Blocker).Count();
    }

    private int GetLinkCount(List<IBoardObject> allBoardObjects)
    {
        IBoardObject startingObject = allBoardObjects[0];
        List<IBoardObject> checkedObjects = new List<IBoardObject>
        {
            startingObject
        };
        RecursivelyChain(startingObject, checkedObjects);

        return checkedObjects.Count;

    }

    private void RecursivelyChain(IBoardObject linkedObject, List<IBoardObject> checkedObjects)
    {
        foreach (IBoardObject connectedObject in linkedObject.ConnectedObjectsAt.Keys)
        {
            if (checkedObjects.Contains(connectedObject))
            {
                continue;
            }
            else
            {
                checkedObjects.Add(connectedObject);
            }

            if (connectedObject.BoardObjectType == BoardObjectType.Panel)
            {
                RecursivelyChain(connectedObject, checkedObjects);
            }
        }
    }
}
