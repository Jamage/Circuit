using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static List<Panel> allPanels = new List<Panel>();
    public float InnerLineWidth = .04f;
    public float BorderLineWidth = .04f;

    private void Awake()
    {
        SetAllLineWidths();
    }

    private void SetAllLineWidths()
    {
        foreach (Panel panel in allPanels)
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

        foreach(Panel panel in allPanels)
        {
            if (panel.IsConnected == false)
            {
                Debug.Log($"{panel.name} not connected");
                allConnected = false;
            }
            else
                Debug.Log($"{panel.name} is connected");
        }

        return allConnected;
    }

    internal static bool IsOccupied(Vector2Int positionIndex)
    {
        bool isOccupied = false;
        foreach(Panel linePanel in allPanels)
        {
            if(linePanel.PositionIndex == positionIndex)
            {
                isOccupied = true;
                break;
            }
        }

        return isOccupied;
    }
}
