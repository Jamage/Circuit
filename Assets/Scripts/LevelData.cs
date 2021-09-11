using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 51)]
public class LevelData : ScriptableObject
{
    public string Name;
    public int RowCount, ColumnCount;
    public List<PanelData> PanelDataList;
    public List<CircuitData> CircuitDataList;
    public List<BlockingData> BlockingDataList;
}
