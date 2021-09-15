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
    private SerializedProperty rowCount, columnCount;

    private void OnEnable()
    {
        panelDataList = serializedObject.FindProperty("PanelDataList");
        circuitDataList = serializedObject.FindProperty("CircuitDataList");
        blockingDataList = serializedObject.FindProperty("BlockingDataList");
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

    private void DrawLinePanels(float width, float height, float startingX, float startingY)
    {
        for (int panelIndex = 0; panelIndex < panelDataList.arraySize; panelIndex++)
        {
            SerializedProperty panel = panelDataList.GetArrayElementAtIndex(panelIndex);
            SerializedProperty vectorProperty = panel.FindPropertyRelative("PositionIndex");
            Vector2Int positionIndex = vectorProperty.vector2IntValue;
            PanelType panelType = (PanelType)panel.FindPropertyRelative("PanelType").enumValueIndex;

            float panelCenterX = startingX + (width / 2) + (width * positionIndex.x);
            float panelCenterY = startingY + (height / 2) + (height * positionIndex.y);
            Vector3 panelCenter = new Vector3(panelCenterX, panelCenterY, 0);
            Vector3 lineStartPos = Vector3.zero;
            Vector3 lineEndPos = Vector3.zero;
            switch (panelType)
            {
                case PanelType.OneTwo:
                    lineStartPos = GetPointOne(positionIndex, width, height, startingX, startingY);
                    lineEndPos = GetPointTwo(positionIndex, width, height, startingX, startingY);
                    break;
                case PanelType.OneThree:
                    lineStartPos = GetPointOne(positionIndex, width, height, startingX, startingY);
                    lineEndPos = GetPointThree(positionIndex, width, height, startingX, startingY);
                    break;
                case PanelType.OneFour:
                    lineStartPos = GetPointOne(positionIndex, width, height, startingX, startingY);
                    lineEndPos = GetPointFour(positionIndex, width, height, startingX, startingY);
                    break;
                case PanelType.TwoThree:
                    lineStartPos = GetPointTwo(positionIndex, width, height, startingX, startingY);
                    lineEndPos = GetPointThree(positionIndex, width, height, startingX, startingY);
                    break;
                case PanelType.TwoFour:
                    lineStartPos = GetPointTwo(positionIndex, width, height, startingX, startingY);
                    lineEndPos = GetPointFour(positionIndex, width, height, startingX, startingY);
                    break;
                case PanelType.ThreeFour:
                    lineStartPos = GetPointThree(positionIndex, width, height, startingX, startingY);
                    lineEndPos = GetPointFour(positionIndex, width, height, startingX, startingY);
                    break;
            }

            DrawLineFromTo(lineStartPos, panelCenter);
            DrawLineFromTo(panelCenter, lineEndPos);
        }
    }

    private void DrawLineFromTo(Vector3 lineStartPos, Vector3 lineEndPos)
    {
        Handles.BeginGUI();
        Handles.color = Color.blue;
        Handles.DrawLine(lineStartPos, lineEndPos, 4);
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
        float xPos = startingX + (positionIndex.x + 1) * width;
        float yPos = startingY + positionIndex.y * height;

        Vector3 pointTwo = new Vector3(xPos, yPos, 0);
        return pointTwo;
    }

    private Vector3 GetPointThree(Vector2Int positionIndex, float width, float height, float startingX, float startingY)
    {
        float xPos = startingX + positionIndex.x * width;
        float yPos = startingY + (positionIndex.y + 1) * height;

        Vector3 pointThree = new Vector3(xPos, yPos, 0);
        return pointThree;
    }

    private Vector3 GetPointFour(Vector2Int positionIndex, float width, float height, float startingX, float startingY)
    {
        float xPos = startingX + (positionIndex.x + 1) * width;
        float yPos = startingY + (positionIndex.y + 1) * height;

        Vector3 pointFour = new Vector3(xPos, yPos, 0);
        return pointFour;
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
