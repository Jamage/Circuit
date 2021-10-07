using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoardSolver
{
    public int rows;
    public int columns;
    private readonly int maxPanels, minPanels;
    //No Panels should have been generated, only circuit points
    public List<IBoardObject> allBoardObjects => BoardObjectManager.allBoardObjects;

    public GameBoardSolver(GameBoard gameBoard)
    {
        rows = gameBoard.rowCount;
        columns = gameBoard.columnCount;
        maxPanels = (rows * columns) - 1;
        minPanels = GetShortestPath();
    }

    private int GetShortestPath()
    {
        List<CircuitPoint> circuitPoints = CircuitManager.allCircuitPoints;
        List<Vector2Int> circuitPositions = new List<Vector2Int>();
        List<Vector2Int> potentialPositions = new List<Vector2Int>();
        Dictionary<Vector2Int, int> sharedPanelPositions = new Dictionary<Vector2Int, int>();


        foreach(CircuitPoint point in circuitPoints)
        {
            circuitPositions.Add(point.PositionIndex);
        }

        int panelMin = circuitPositions.Count;

        foreach (Vector2Int position in circuitPositions)
        {
            List<Vector2Int> surroundingPositions = GetSurroundingPositions(position);
            foreach (Vector2Int surroundingPosition in surroundingPositions)
            {
                if (potentialPositions.Contains(surroundingPosition) == false)
                {
                    potentialPositions.Add(surroundingPosition);
                }
                else if(sharedPanelPositions.ContainsKey(surroundingPosition))
                {
                    sharedPanelPositions.Add(surroundingPosition, 1);
                }
                else
                {
                    sharedPanelPositions[surroundingPosition]++;
                }
            }
        }

        panelMin -= sharedPanelPositions.Keys.Count;


        return panelMin;
    }

    private List<Vector2Int> GetSurroundingPositions(Vector2Int circuitPosition)
    {
        List<Vector2Int> surroundingPositions = new List<Vector2Int>();
        if (circuitPosition == Vector2Int.zero)
        {
            surroundingPositions.Add(circuitPosition);
        }
        else if(circuitPosition == new Vector2Int(rows, columns))
        {
            surroundingPositions.Add(circuitPosition - Vector2Int.one);
        }
        else if(circuitPosition.x == 0)
        {
            surroundingPositions.Add(circuitPosition);
            surroundingPositions.Add(circuitPosition - Vector2Int.up);
        }
        else if(circuitPosition.y == 0)
        {
            surroundingPositions.Add(circuitPosition);
            surroundingPositions.Add(circuitPosition - Vector2Int.right);
        }
        else if(circuitPosition.x == columns)
        {
            surroundingPositions.Add(circuitPosition - Vector2Int.right);
            surroundingPositions.Add(circuitPosition - Vector2Int.one);
        }
        else if(circuitPosition.y == rows)
        {
            surroundingPositions.Add(circuitPosition - Vector2Int.up);
            surroundingPositions.Add(circuitPosition - Vector2Int.one);
        }
        else
        {
            surroundingPositions.Add(circuitPosition);
            surroundingPositions.Add(circuitPosition - Vector2Int.up);
            surroundingPositions.Add(circuitPosition - Vector2Int.right);
            surroundingPositions.Add(circuitPosition - Vector2Int.one);
        }

        return surroundingPositions;
    }

    //GRID 2x2
    // 0,0  1,0  2,0
    // 0,1  1,1  2,1
    // 0,2  1,2  2,2

    /*Points at 0,0 & 2,2
         0--|--|
         |--|--|
         |--|--0
        Direct Path is 0,0  1,1  2,2
        Max Panel Count is (rows * columns) - 1 = 3
        Do I need A*?
        Line Panels always go to center before the next corner
    */

    public void Solve()
    {
        IBoardObject startingCircuit = allBoardObjects.First(item => item.BoardObjectType == BoardObjectType.Circuit);
        Vector2Int startingPosition = startingCircuit.PositionIndex;

    }

}
