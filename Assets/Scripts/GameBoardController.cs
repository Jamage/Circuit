﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GameBoardController
{
    public static int height, width;
    public static int columnCount, rowCount;
    public static Vector2 positionStart;
    public static float maxHeightScale = 7.4f;
    public static float maxWidthScale = 14.8f;

    public static void Reset(GameBoard gameBoard)
    {
        height = gameBoard.Height;
        width = gameBoard.Width;
        columnCount = gameBoard.columnCount;
        rowCount = gameBoard.rowCount;
        positionStart = new Vector2((-columnCount * (float)width) / 2, ((float)rowCount * (float)height) / 2);
        SetScale();
    }

    private static void SetScale()
    {
        maxHeightScale /= (rowCount * height);
        maxWidthScale /= (columnCount * width);
        if (maxHeightScale < maxWidthScale)
        {
            maxWidthScale = maxHeightScale;
        }
        else
            maxHeightScale = maxWidthScale;
    }

    internal static Vector3 GetPositionFor(Vector2Int positionIndex)
    {
        return new Vector3(XPositionForColumn(positionIndex.x), YPositionForRow(positionIndex.y), 0);
    }

    internal static Vector3 GetNewPositionFor(Vector2Int positionIndex)
    {
        //Debug.Log($"Position: ({positionIndex.x},{positionIndex.y}) Pos: ({XPositionForColumn(positionIndex.x) / 2}, {YPositionForRow(positionIndex.y) / 2})");
        return new Vector3(XLinePositionForColumn(positionIndex.x), YLinePositionForRow(positionIndex.y) , 0);
    }

    public static Vector2Int GetRandomIndexPosition()
    {
        return new Vector2Int(RandomColumnIndex(), RandomRowIndex());
    }

    public static Vector2Int GetRandomIndexFor(Edge edge)
    {
        int columnIndex;
        int rowIndex;

        switch (edge)
        {
            case Edge.Left:
            default:
                columnIndex = 0;
                rowIndex = RandomRowIndex();
                break;
            case Edge.Right:
                columnIndex = columnCount;
                rowIndex = RandomRowIndex();
                break;
            case Edge.Top:
                columnIndex = RandomColumnIndex();
                rowIndex = 0;
                break;
            case Edge.Bottom:
                columnIndex = RandomColumnIndex();
                rowIndex = rowCount;
                break;
        }

        return new Vector2Int(columnIndex, rowIndex);
    }

    internal static Vector2Int GetRandomInnerIndexPosition()
    {
        int innerX = UnityEngine.Random.Range(1, columnCount);
        int innerY = UnityEngine.Random.Range(1, rowCount);

        return new Vector2Int(innerX, innerY);
    }

    public static Vector3 GetRandomPosition()
    {
        float posX = RandomXPosition();
        float posY = RandomYPosition();

        return new Vector3(posX, posY, 0);
    }

    public static float XPositionForColumn(int column)
    {
        return positionStart.x + (column * width);
    }

    //-3 + (2 * 1) = -1 / 2 = -.5
    //

    public static float YPositionForRow(int row)
    {
        return positionStart.y - (row * height);
    }

    public static float XLinePositionForColumn(int column)
    {
        return positionStart.x + (column * 3);
    }

    public static float YLinePositionForRow(int row)
    {
        return positionStart.y - (row * 1.5f);
    }

    public static float RandomXPosition()
    {
        int column = RandomColumnIndex();
        return XPositionForColumn(column);
    }

    public static float RandomYPosition()
    {
        int row = RandomRowIndex();
        return YPositionForRow(row);
    }

    private static int RandomColumnIndex()
    {
        return UnityEngine.Random.Range(0, columnCount + 1);
    }

    private static int RandomRowIndex()
    {
        return UnityEngine.Random.Range(0, rowCount + 1);
    }

    internal static List<Vector2Int> GetConnectingPoints(Vector2Int positionIndex, LinePanelType panelType)
    {
        List<Vector2Int> connectingPoints = new List<Vector2Int>();

        //Panel Positions
        //1  2  3
        //4  5  6
        //7  8  9

        if (panelType.HasFlag(LinePanelType.One))
            connectingPoints.Add(new Vector2Int(positionIndex.x, positionIndex.y));
        //if (panelType.HasFlag(LinePanelType.Two))
            //connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y));
        if (panelType.HasFlag(LinePanelType.Three))
            connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y));
        //if (panelType.HasFlag(LinePanelType.Four))
            //connectingPoints.Add(new Vector2Int(positionIndex.x, positionIndex.y + 1));
        //if(panelType.HasFlag(LinePanelType.Five))
            //connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y + 1));
        //if (panelType.HasFlag(LinePanelType.Six))
            //connectingPoints.Add(new Vector2Int(positionIndex.x + 2, positionIndex.y + 1));
        if (panelType.HasFlag(LinePanelType.Seven))
            connectingPoints.Add(new Vector2Int(positionIndex.x, positionIndex.y + 1));
        //if (panelType.HasFlag(LinePanelType.Eight))
            //connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y + 1));
        if (panelType.HasFlag(LinePanelType.Nine))
            connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y + 1));

        return connectingPoints;
    }
}
