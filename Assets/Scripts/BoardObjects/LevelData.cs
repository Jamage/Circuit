using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 51)]
public class LevelData : ScriptableObject
{
    public string Name => name;
    public int RowCount, ColumnCount;
    public List<LinePanelData> LinePanelDataList;
    public List<CircuitData> CircuitDataList;
    public List<BlockingData> BlockingDataList;
    public List<BlockingPanelData> BlockingPanelDataList;
}
