using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(LevelData))]
public class LevelDataInspector : Editor
{
    private SerializedProperty panelDataList;
    private SerializedProperty circuitDataList;
    private SerializedProperty blockingDataList;
    private SerializedProperty blockingPanelDataList;
    private SerializedProperty rowCount, columnCount;

    private void OnEnable()
    {
        panelDataList = serializedObject.FindProperty("LinePanelDataList");
        circuitDataList = serializedObject.FindProperty("CircuitDataList");
        blockingDataList = serializedObject.FindProperty("BlockingDataList");
        blockingPanelDataList = serializedObject.FindProperty("BlockingPanelDataList");
        rowCount = serializedObject.FindProperty("RowCount");
        columnCount = serializedObject.FindProperty("ColumnCount");
    }

    // Helper method to get a square rectangle of the correct aspect ratio
    private Rect GetCenteredRect(Rect rect, float aspect = 1f)
    {
        Vector2 size = rect.size;
        size.x = Mathf.Min(size.x, rect.size.y * aspect);
        size.y = Mathf.Min(size.y, rect.size.x / aspect);

        Vector2 pos = rect.min + (rect.size - size) * 0.5f;
        return new Rect(pos, size);
    }

    public override bool HasPreviewGUI()
    {
        return true;
    }


    public override GUIContent GetPreviewTitle()
    {
        return new GUIContent("Level Preview");
    }

    public override void DrawPreview(Rect rect)
    {
        rect = GetCenteredRect(rect);

        // Draw background of the rect we plot points in
        EditorGUI.DrawRect(rect, new Color(0.8f, 0.8f, 0.8f));
        
        float width = rect.size.x / (columnCount.intValue + 1);
        float height = rect.size.y / (rowCount.intValue + 1) / 2;
        float startingX = rect.position.x + (width / 2);
        float startingY = rect.position.y + (height / 2) + (rect.size.y / 4);

        DrawGridColumns(width, height, startingX, startingY);
        DrawGridRows(width, height, startingX, startingY);
        DrawBlockingPanels(width, height, startingX, startingY);
        DrawLinePanels(width, height, startingX, startingY);
        DrawBlockingPoints(width, height, startingX, startingY);
        DrawCircuitPoints(width, height, startingX, startingY);
    }

    private void DrawCircuitPoints(float width, float height, float startingX, float startingY)
    {
        for(int circuitIndex = 0; circuitIndex < circuitDataList.arraySize; circuitIndex++)
        {
            SerializedProperty circuitData = circuitDataList.GetArrayElementAtIndex(circuitIndex);
            SerializedProperty vectorProperty = circuitData.FindPropertyRelative("PositionIndex");
            Vector2Int positionIndex = vectorProperty.vector2IntValue;
            Vector3 pointPosition = GetPointOne(positionIndex, width, height, startingX, startingY);
            float dotSize = 10;
            float halfDotSize = dotSize / 2;
            Rect drawingRect = new Rect(
                pointPosition.x - halfDotSize,
                pointPosition.y - halfDotSize, dotSize, dotSize);

            EditorGUI.DrawRect(drawingRect, Color.yellow);
        }
    }

    private void DrawBlockingPoints(float width, float height, float startingX, float startingY)
    {
        for (int blockingIndex = 0; blockingIndex < blockingDataList.arraySize; blockingIndex++)
        {
            SerializedProperty blockingData = blockingDataList.GetArrayElementAtIndex(blockingIndex);
            SerializedProperty vectorProperty = blockingData.FindPropertyRelative("PositionIndex");
            Vector2Int positionIndex = vectorProperty.vector2IntValue;
            Vector3 pointPosition = GetPointOne(positionIndex, width, height, startingX, startingY);
            float dotSize = 14;
            float halfDotSize = dotSize / 2;
            Rect drawingRect = new Rect(
                pointPosition.x - halfDotSize,
                pointPosition.y - halfDotSize, dotSize, dotSize);

            EditorGUI.DrawRect(drawingRect, Color.red);
        }
    }

    private void DrawBlockingPanels(float width, float height, float startingX, float startingY)
    {
        for (int blockingPanelIndex = 0; blockingPanelIndex < blockingPanelDataList.arraySize; blockingPanelIndex++)
        {
            SerializedProperty blockingPanel = blockingPanelDataList.GetArrayElementAtIndex(blockingPanelIndex);
            SerializedProperty vectorProperty = blockingPanel.FindPropertyRelative("PositionIndex");
            Vector2Int positionIndex = vectorProperty.vector2IntValue;

            Vector3 lineStartPos = GetPointOne(positionIndex, width, height, startingX, startingY);
            Vector3 lineEndPos = GetPointNine(positionIndex, width, height, startingX, startingY);
            DrawLineFromTo(lineStartPos, lineEndPos, Color.black);
            lineStartPos = GetPointSeven(positionIndex, width, height, startingX, startingY);
            lineEndPos = GetPointThree(positionIndex, width, height, startingX, startingY);
            DrawLineFromTo(lineStartPos, lineEndPos, Color.black);
        }
    }

    private void DrawLinePanels(float width, float height, float startingX, float startingY)
    {
        for (int panelIndex = 0; panelIndex < panelDataList.arraySize; panelIndex++)
        {
            SerializedProperty panel = panelDataList.GetArrayElementAtIndex(panelIndex);
            SerializedProperty vectorProperty = panel.FindPropertyRelative("PositionIndex");
            Vector2Int positionIndex = vectorProperty.vector2IntValue;
            LinePanelType panelType = (LinePanelType)panel.FindPropertyRelative("PanelType").intValue;
            
            float panelCenterX = startingX + (width / 2) + (width * positionIndex.x);
            float panelCenterY = startingY + (height / 2) + (height * positionIndex.y);
            Vector3 panelCenter = new Vector3(panelCenterX, panelCenterY, 0);

            if (panelType.HasFlag(LinePanelType.One))
            {
                Vector3 lineStartPos = GetPointOne(positionIndex, width, height, startingX, startingY);
                DrawLineFromTo(lineStartPos, panelCenter, Color.blue);
            }
            if (panelType.HasFlag(LinePanelType.Two))
            {
                Vector3 lineStartPos = GetPointTwo(positionIndex, width, height, startingX, startingY);
                DrawLineFromTo(lineStartPos, panelCenter, Color.blue);
            }
            if (panelType.HasFlag(LinePanelType.Three))
            {
                Vector3 lineStartPos = GetPointThree(positionIndex, width, height, startingX, startingY);
                DrawLineFromTo(lineStartPos, panelCenter, Color.blue);
            }
            if (panelType.HasFlag(LinePanelType.Four))
            {
                Vector3 lineStartPos = GetPointFour(positionIndex, width, height, startingX, startingY);
                DrawLineFromTo(lineStartPos, panelCenter, Color.blue);
            }
            if (panelType.HasFlag(LinePanelType.Six))
            {
                Vector3 lineStartPos = GetPointSix(positionIndex, width, height, startingX, startingY);
                DrawLineFromTo(lineStartPos, panelCenter, Color.blue);
            }
            if (panelType.HasFlag(LinePanelType.Seven))
            {
                Vector3 lineStartPos = GetPointSeven(positionIndex, width, height, startingX, startingY);
                DrawLineFromTo(lineStartPos, panelCenter, Color.blue);
            }
            if (panelType.HasFlag(LinePanelType.Eight))
            {
                Vector3 lineStartPos = GetPointEight(positionIndex, width, height, startingX, startingY);
                DrawLineFromTo(lineStartPos, panelCenter, Color.blue);
            }
            if (panelType.HasFlag(LinePanelType.Nine))
            {
                Vector3 lineStartPos = GetPointNine(positionIndex, width, height, startingX, startingY);
                DrawLineFromTo(lineStartPos, panelCenter, Color.blue);
            }

        }
    }

    private void DrawLineFromTo(Vector3 lineStartPos, Vector3 lineEndPos, Color color, float thickness = 4f)
    {
        Handles.BeginGUI();
        Handles.color = color;
        Handles.DrawLine(lineStartPos, lineEndPos, thickness);
        Handles.EndGUI();
    }

    private Vector3 GetPointOne(Vector2Int positionIndex, float width, float height, float startingX, float startingY)
    {
        float xPos = startingX + positionIndex.x * width;
        float yPos = startingY + positionIndex.y * height;

        Vector3 pointOne = new Vector3(xPos, yPos, 0);
        return pointOne;
    }

    private Vector3 GetPointTwo(Vector2Int positionIndex, float width, float height, float startingX, float startingY)
    {
        float xPos = startingX + (positionIndex.x + .5f) * width;
        float yPos = startingY + positionIndex.y * height;

        Vector3 pointTwo = new Vector3(xPos, yPos, 0);
        return pointTwo;
    }

    private Vector3 GetPointThree(Vector2Int positionIndex, float width, float height, float startingX, float startingY)
    {
        float xPos = startingX + (positionIndex.x + 1) * width;
        float yPos = startingY + positionIndex.y * height;

        Vector3 pointThree = new Vector3(xPos, yPos, 0);
        return pointThree;
    }

    private Vector3 GetPointFour(Vector2Int positionIndex, float width, float height, float startingX, float startingY)
    {
        float xPos = startingX + positionIndex.x * width;
        float yPos = startingY + (positionIndex.y + .5f) * height;

        Vector3 pointFour = new Vector3(xPos, yPos, 0);
        return pointFour;
    }

    private Vector3 GetPointSix(Vector2Int positionIndex, float width, float height, float startingX, float startingY)
    {
        float xPos = startingX + (positionIndex.x + 1) * width;
        float yPos = startingY + (positionIndex.y + .5f) * height;

        Vector3 pointSix = new Vector3(xPos, yPos, 0);
        return pointSix;
    }

    private Vector3 GetPointSeven(Vector2Int positionIndex, float width, float height, float startingX, float startingY)
    {
        float xPos = startingX + positionIndex.x * width;
        float yPos = startingY + (positionIndex.y + 1) * height;

        Vector3 pointSeven = new Vector3(xPos, yPos, 0);
        return pointSeven;
    }

    private Vector3 GetPointEight(Vector2Int positionIndex, float width, float height, float startingX, float startingY)
    {
        float xPos = startingX + (positionIndex.x + .5f) * width;
        float yPos = startingY + (positionIndex.y + 1) * height;

        Vector3 pointEight = new Vector3(xPos, yPos, 0);
        return pointEight;
    }

    private Vector3 GetPointNine(Vector2Int positionIndex, float width, float height, float startingX, float startingY)
    {
        float xPos = startingX + (positionIndex.x + 1) * width;
        float yPos = startingY + (positionIndex.y + 1) * height;

        Vector3 pointNine = new Vector3(xPos, yPos, 0);
        return pointNine;
    }

    private void DrawGridRows(float width, float height, float startingX, float startingY)
    {
        for (int rowIndex = 0; rowIndex < rowCount.intValue + 1; rowIndex++)
        {
            Handles.BeginGUI();
            Handles.color = Color.red;
            float posY = startingY + (rowIndex * height);
            Vector3 startingPoint = new Vector3(startingX, posY, 0);
            Vector3 endPoint = new Vector3(startingX + (columnCount.intValue * width), posY, 0);
            Handles.DrawLine(startingPoint, endPoint);
            Handles.EndGUI();
        }
    }

    private void DrawGridColumns(float width, float height, float startingX, float startingY)
    {
        for (int columnIndex = 0; columnIndex < columnCount.intValue + 1; columnIndex++)
        {
            Handles.BeginGUI();
            Handles.color = Color.red;
            float posX = startingX + (columnIndex * width);
            Vector3 startingPoint = new Vector3(posX, startingY, 0);
            Vector3 endPoint = new Vector3(posX, startingY + (rowCount.intValue * height), 0);
            Handles.DrawLine(startingPoint, endPoint);
            Handles.EndGUI();
        }
    }
}
