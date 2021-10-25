using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static List<LinePanel> allLinePanels = new List<LinePanel>();
    public LinePanel linePanelPrefab;
    public static LinePanel staticLinePanelPrefab;
    public float InnerLineWidth = .04f;
    public float BorderLineWidth = .04f;

    private void Awake()
    {
        staticLinePanelPrefab = linePanelPrefab;
        SetAllLineWidths();
    }

    private void SetAllLineWidths()
    {
        foreach(LineRenderer line in staticLinePanelPrefab.innerLines)
                line.widthCurve = new AnimationCurve(new Keyframe(0, InnerLineWidth));
        foreach (LineRenderer line in staticLinePanelPrefab.borderLines)
            line.widthCurve = new AnimationCurve(new Keyframe(0, BorderLineWidth));

        foreach (LinePanel panel in allLinePanels)
        {
            foreach (LineRenderer line in panel.innerLines)
            {
                line.widthCurve = new AnimationCurve(new Keyframe(0, InnerLineWidth));
            }

            foreach (LineRenderer line in panel.borderLines)
            {
                line.widthCurve = new AnimationCurve(new Keyframe(0, BorderLineWidth));
            }
        }
    }

    public static bool AreAllPanelsConnected()
    {
        bool allConnected = true;

        foreach (LinePanel panel in allLinePanels)
        {
            if (panel.IsConnected == false)
            {
                //Debug.Log($"{panel.name} not connected");
                allConnected = false;
            }
            else
            {
                //Debug.Log($"{panel.name} is connected");
            }
        }

        return allConnected;
    }

    internal static bool IsOccupied(Vector2Int positionIndex, out LinePanel occupyingPanel)
    {
        occupyingPanel = null;
        bool isOccupied = false;

        foreach (LinePanel linePanel in allLinePanels)
        {
            if (linePanel.PositionIndex == positionIndex)
            {
                isOccupied = true;
                occupyingPanel = linePanel;
                break;
            }
        }

        return isOccupied;
    }

    internal static LinePanel GetLinePanel()
    {
        return staticLinePanelPrefab;
    }
}
