using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlockingPanelData
{
    public Vector2Int PositionIndex;
    public BlockingPanelData()
    {
        PositionIndex = Vector2Int.zero;
    }
}
