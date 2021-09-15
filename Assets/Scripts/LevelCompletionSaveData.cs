using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelCompletionSaveData
{
    public string LevelName;
    public bool IsComplete;

    public LevelCompletionSaveData(LevelData levelData)
    {
        LevelName = levelData.Name;
        IsComplete = false;
    }
}
