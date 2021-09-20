using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameBoard : MonoBehaviour
{
    public int columnCount = 5;
    public int rowCount = 8;
    public BackgroundPanel BackgroundPanelPrefab;
    private readonly int backgroundZPos = 1;
    [Range(2, 5), SerializeField] private int edgeCircuitPointCount = 2;
    [Range(0, 5), SerializeField] private int innerCircuitPointCount = 0;
    [Range(0, 5), SerializeField] private int blockingPointCount = 0;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public bool isGenerated = false;
    private readonly List<GameObject> boardObjectList = new List<GameObject>();
    
    protected void Awake()
    {
        Width = 2;
        Height = 1;
        GameBoardController.Reset(this);
        if (isGenerated)
            Generate();
    }

    public void Generate()
    {
        GenerateBackgroundGrid();
        GenerateEdgeCircuitPoints();
        GenerateInnerCircuitPoints();
        GenerateBlockingPoints();
        GenerateSolutionPanels();
        TurnManager.ResetTurnCounter();
    }

    public void Setup(LevelData data)
    {
        Clear();
        rowCount = data.RowCount;
        columnCount = data.ColumnCount;
        GameBoardController.Reset(this);
        GenerateBackgroundGrid(data.BlockingPanelDataList);
        GenerateCircuitPoints(data.CircuitDataList);
        GenerateBlockingPoints(data.BlockingDataList);
        GeneratePanels(data.PanelDataList);
        TurnManager.ResetTurnCounter();
    }

    private void Clear()
    {
        foreach(GameObject boardObject in boardObjectList)
        {
            Destroy(boardObject);
        }
        boardObjectList.Clear();
    }

    private void GeneratePanels(List<PanelData> panelDataList)
    {
        foreach (PanelData panelData in panelDataList)
        {
            Panel newPanel = Panel.New(panelData);
            boardObjectList.Add(newPanel.gameObject);
        }
    }

    private void GenerateCircuitPoints(List<CircuitData> circuitDataList)
    {
        foreach (CircuitData circuitData in circuitDataList)
        {
            CircuitPoint newPoint = CircuitPoint.New(circuitData);
            boardObjectList.Add(newPoint.gameObject);
        }
    }

    private void GenerateBlockingPoints(List<BlockingData> blockingDataList)
    {
        foreach (BlockingData blockingData in blockingDataList)
        {
            BlockingPoint newPoint = BlockingPoint.New(blockingData);
            boardObjectList.Add(newPoint.gameObject);
        }
    }

    private void GenerateBlockingPoints()
    {
        for (int i = 0; i < blockingPointCount; i++)
        {
            BlockingPoint newPoint = BlockingPoint.New(new BlockingData());
            newPoint.name = $"Block {i}";
            newPoint.transform.parent = transform;
            newPoint.InitializeInner();
            boardObjectList.Add(newPoint.gameObject);
        }
    }

    private void GenerateEdgeCircuitPoints()
    {
        for (int i = 0; i < edgeCircuitPointCount; i++)
        {
            CircuitPoint edgePoint = CircuitPoint.New(new CircuitData());
            edgePoint.name = $"Edge Point {i}";
            edgePoint.InitializeEdge();
            edgePoint.transform.parent = transform;
            boardObjectList.Add(edgePoint.gameObject);
        }
    }

    private void GenerateInnerCircuitPoints()
    {
        for (int i = 0; i < innerCircuitPointCount; i++)
        {
            CircuitPoint innerPoint = CircuitPoint.New(new CircuitData());
            innerPoint.name = $"Inner Point {i}";
            innerPoint.transform.parent = transform;
            innerPoint.InitializeInner();
            boardObjectList.Add(innerPoint.gameObject);
        }
    }

    private void GenerateSolutionPanels()
    {

    }

    private void GenerateBackgroundGrid()
    {
        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                BackgroundPanel bgPanel = Instantiate(BackgroundPanelPrefab, transform.position, BackgroundPanelPrefab.transform.rotation);
                bgPanel.PositionIndex = new Vector2Int(column, row);

                bgPanel.transform.position =
                    new Vector3(
                        GameBoardController.XPositionForColumn(column),
                        GameBoardController.YPositionForRow(row),
                        backgroundZPos);

                bgPanel.transform.parent = transform;
                boardObjectList.Add(bgPanel.gameObject);
            }
        }
    }

    private void GenerateBackgroundGrid(List<BlockingPanelData> blockingPanelDataList)
    {
        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                if (blockingPanelDataList.Exists(panel => panel.PositionIndex.x == column && panel.PositionIndex.y == row))
                    continue;

                BackgroundPanel bgPanel = Instantiate(BackgroundPanelPrefab, transform.position, BackgroundPanelPrefab.transform.rotation);
                bgPanel.PositionIndex = new Vector2Int(column, row);

                bgPanel.transform.position =
                    new Vector3(
                        GameBoardController.XPositionForColumn(column),
                        GameBoardController.YPositionForRow(row),
                        backgroundZPos);

                bgPanel.transform.parent = transform;
                boardObjectList.Add(bgPanel.gameObject);
            }
        }
    }
}
