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
    }

    public void Setup(LevelData data)
    {
        rowCount = data.RowCount;
        columnCount = data.ColumnCount;
        GameBoardController.Reset(this);
        GenerateBackgroundGrid();
        GenerateCircuitPoints(data.CircuitDataList);
        GenerateBlockingPoints(data.BlockingDataList);
        GeneratePanels(data.PanelDataList);
    }

    private void GeneratePanels(List<PanelData> panelDataList)
    {
        foreach (PanelData panelData in panelDataList)
        {
            Panel.New(panelData);
        }
    }

    private void GenerateCircuitPoints(List<CircuitData> circuitDataList)
    {
        foreach (CircuitData circuitData in circuitDataList)
        {
            CircuitPoint.New(circuitData);
        }
    }

    private void GenerateBlockingPoints(List<BlockingData> blockingDataList)
    {
        foreach (BlockingData blockingData in blockingDataList)
        {
            BlockingPoint.New(blockingData);
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
            }
        }
    }
}
