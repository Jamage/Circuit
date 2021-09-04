using System;
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

    public static void Reset()
    {
        height = GameBoard.Instance.Height;
        width = GameBoard.Instance.Width;
        columnCount = GameBoard.Instance.columnCount;
        rowCount = GameBoard.Instance.rowCount;
        positionStart = new Vector2((-columnCount * width) / 2, (rowCount * height) / 2);
    }

    internal static Vector3 GetPositionFor(Vector2 positionIndex)
    {
        return new Vector3(XPositionForColumn((int)positionIndex.x), YPositionForRow((int)positionIndex.y), 0);
    }

    public static Vector2Int GetRandomIndexPosition()
    {
        return new Vector2Int(RandomColumnIndex(), RandomRowIndex());
    }

    public static Vector2Int GetIndexFor(Edge edge)
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

    public static float YPositionForRow(int row)
    {
        return positionStart.y - (row * height);
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

    internal static List<Vector2Int> GetConnectingPoints(Vector2Int positionIndex, PanelType panelType)
    {
        List<Vector2Int> connectingPoints = new List<Vector2Int>();
        
        //Panel Corners
        //1  2
        //3  4

        switch (panelType)
        {
            case PanelType.OneTwo:
                connectingPoints.Add(new Vector2Int(positionIndex.x, positionIndex.y));
                connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y));
                break;
            case PanelType.OneThree:
                connectingPoints.Add(new Vector2Int(positionIndex.x, positionIndex.y));
                connectingPoints.Add(new Vector2Int(positionIndex.x, positionIndex.y + 1));
                break;
            case PanelType.OneFour:
                connectingPoints.Add(new Vector2Int(positionIndex.x, positionIndex.y));
                connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y + 1));
                break;
            case PanelType.TwoThree:
                connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y));
                connectingPoints.Add(new Vector2Int(positionIndex.x, positionIndex.y + 1));
                break;
            case PanelType.TwoFour:
                connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y));
                connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y + 1));
                break;
            case PanelType.ThreeFour:
                connectingPoints.Add(new Vector2Int(positionIndex.x, positionIndex.y + 1));
                connectingPoints.Add(new Vector2Int(positionIndex.x + 1, positionIndex.y + 1));
                break;
        }

        return connectingPoints;
    }
}
