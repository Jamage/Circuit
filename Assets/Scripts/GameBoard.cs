using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : GenericSingletonClass<GameBoard>
{
    public int columnCount = 5;
    public int rowCount = 8;
    public BackgroundPanel BackgroundPanel;
    public CircuitPoint circuitPoint;
    public BlockingPoint blockingPoint;
    private readonly List<CircuitPoint> circuitPoints = new List<CircuitPoint>();
    private readonly List<BlockingPoint> blockingPoints = new List<BlockingPoint>();
    public int backgroundZPos = 1;
    [Range(2, 5), SerializeField] private int edgeCircuitPointCount = 2;
    [Range(0, 5), SerializeField] private int innerCircuitPointCount = 0;
    [Range(0, 5), SerializeField] private int blockingPointCount = 0;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public bool isGenerating = true;

    protected override void Awake()
    {
        base.Awake();
        Width = 2;
        Height = 1;
        GameBoardController.Reset();
    }

    void Start()
    {
        if (isGenerating)
        {
            GenerateGrid();
            GenerateEdgeCircuitPoints();
            GenerateInnerCircuitPoints();
            GenerateBlockingPoints();
            GenerateSolution();
        }
        else
        {

        }
    }

    private void GenerateBlockingPoints()
    {
        for(int i = 0; i < blockingPointCount; i++)
        {
            BlockingPoint newPoint = Instantiate(blockingPoint, transform.position, blockingPoint.transform.rotation);
            newPoint.name = $"Block {i}";
            newPoint.transform.parent = transform;
            blockingPoints.Add(newPoint);
        }
    }

    private void GenerateEdgeCircuitPoints()
    {
        for (int i = 0; i < edgeCircuitPointCount; i++)
        {
            CircuitPoint edgePoint = Instantiate(circuitPoint, transform.position, circuitPoint.transform.rotation);
            edgePoint.InitializeEdge();
            edgePoint.name = $"Point {i}";
            edgePoint.transform.parent = transform;
            circuitPoints.Add(edgePoint);
        }
    }

    private void GenerateInnerCircuitPoints()
    {
        for (int i = 0; i < innerCircuitPointCount; i++)
        {
            CircuitPoint innerPoint = Instantiate(circuitPoint, transform.position, circuitPoint.transform.rotation);
            innerPoint.InitializeInner();
            innerPoint.name = $"Inner Point {i}";
            innerPoint.transform.parent = transform;
            circuitPoints.Add(innerPoint);
        }
    }

    private void GenerateSolution()
    {
        
    }

    private void GenerateGrid()
    {
        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                BackgroundPanel bgPanel = Instantiate(BackgroundPanel, transform.position, BackgroundPanel.transform.rotation);
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
